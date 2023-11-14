using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DatabaseLayer;
using System.Globalization;
using CRMUtilities;
using Newtonsoft.Json;
using System.Xml;
using System.Reflection;

namespace BusinessLayer
{
    // Oct 24 2019 Process/Send Reitzel related sms

    public class ReitzelSms : ISMSType
    {
        public void Process(long tenantId, TenantSmsType smsType, DateTime currentIstDateTime,
            string smsRequestLogFile, string smsResponseLogFile)
        {
            if (smsType == null)
            {
                return;
            }

            string guid = Guid.NewGuid().ToString();
            Business.LogError($"{nameof(ReitzelSms)}", $"TenantId: {tenantId} | SmsType: {smsType.TypeName} | Guid: {guid}");

            string actualSMSText = smsType.MessageText;
            if (String.IsNullOrEmpty(actualSMSText))
            {
                Business.LogError($"{nameof(ReitzelSms)}", $"{guid} : Invalid / Empty Message Text Format");
                return;
            }

            do
            {
                // get list of SMS that are pending 
                IEnumerable<TenantSmsData> smsDataItems = DBLayer.GetTenantSMSData(1, tenantId, smsType.TypeName);
                if (smsDataItems == null || smsDataItems.Count() == 0)
                {
                    break;
                }

                TenantSmsData tenantSmsData = smsDataItems.First();

                Business.LogError($"{nameof(ReitzelSms)}", $"{guid} : Preparing to send SMS for TenantSmsData Id {tenantSmsData.Id}", ">");
                Business.LogError($"{nameof(ReitzelSms)}", tenantSmsData.MessageData);

                SmsData smsData = null;
                try
                {
                    // get the list of placeholders - items that need to be replaced in message text
                    IEnumerable<string> placeHolderList = JsonConvert.DeserializeObject<IEnumerable<string>>(smsType.PlaceHolders);

                    string messageDataAsJson = "";
                    // retrive placeholder values
                    if (tenantSmsData.DataType.Equals("XML", StringComparison.OrdinalIgnoreCase))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(tenantSmsData.MessageData);
                        messageDataAsJson = JsonConvert.SerializeXmlNode(doc);
                        OuterSmsData outerSmsData = JsonConvert.DeserializeObject<OuterSmsData>(messageDataAsJson);
                        smsData = outerSmsData.Row;
                    }
                    else // JSON
                    {
                        messageDataAsJson = tenantSmsData.MessageData;
                        smsData = JsonConvert.DeserializeObject<SmsData>(messageDataAsJson);
                    }

                    // ItemDetails is put from sproc, by joining data from multiple rows
                    // hence remove trailing separator
                    if (String.IsNullOrEmpty(smsData?.ItemDetails ?? "") == false)
                    {
                        smsData.ItemDetails = smsData.ItemDetails.Trim(new char[] { ' ', '|', ',' });
                    }

                    // Create dictionary of smsData
                    Dictionary<string, object> nameValues = new Dictionary<string, object>();
                    PropertyInfo[] propertyInfo = smsData.GetType().GetProperties();
                    for (int i = 0; i < propertyInfo.Length; i++)
                    {
                        if (propertyInfo[i].CanRead)
                        {
                            nameValues[propertyInfo[i].Name] = propertyInfo[i].GetValue(smsData);
                        }
                    }

                    // check to see if there is a value for CompanyName
                    nameValues["CompanyName"] = Utils.SiteConfigData.Caption;

                    // pick up sms template and replace values in there;
                    actualSMSText = smsType.MessageText;

                    if (placeHolderList == null || placeHolderList.Count() == 0)
                    {
                        // no items listed to replace
                    }
                    else
                    {
                        // now start replacing values
                        foreach (var ph in placeHolderList)
                        {
                            object phValue;
                            nameValues.TryGetValue(ph, out phValue);

                            if (phValue == null)
                            {
                                continue;
                            }

                            string parsedValue;
                            if (phValue.GetType().Name.Equals("Decimal", StringComparison.OrdinalIgnoreCase))
                            {
                                parsedValue = $"{(Decimal)phValue:N2}";
                            }
                            else if (phValue.GetType().Name.Equals("Double", StringComparison.OrdinalIgnoreCase))
                            {
                                parsedValue = $"{(Double)phValue:N2}";
                            }
                            else if (phValue.GetType().Name.Equals("Int64", StringComparison.OrdinalIgnoreCase))
                            {
                                parsedValue = $"{(long)phValue}";
                            }
                            else if (phValue.GetType().Name.Equals("Int32", StringComparison.OrdinalIgnoreCase))
                            {
                                parsedValue = $"{(int)phValue}";
                            }
                            else
                            {
                                parsedValue = (string)phValue;
                            }

                            actualSMSText = actualSMSText.Replace($"{{{ph}}}", parsedValue);
                        }
                    }

                    List<string> phones = new List<string> { smsData.PhoneNumber };

                    long smsLogId = 0;
                    bool status = Business.SendSMS(tenantId, phones, actualSMSText, currentIstDateTime, smsType.TypeName, "Auto",
                        Utils.SiteConfigData.OrderSMSToCustomerTemplate,
                        out smsLogId);
                    Business.LogError($"{nameof(ReitzelSms)}", $"{guid} : {status}; SMSLogId {smsLogId}", ">");

                    // at the end unlock + mark it as processed for SMs as well.
                    DBLayer.MarkSmsDataRecord(tenantSmsData.Id, true, false, "");
                }
                catch(Exception ex)
                {
                    Business.LogError($"{nameof(ReitzelSms)}", ex);
                    DBLayer.MarkSmsDataRecord(tenantSmsData.Id, false, true, ex.Message);
                }
            } while (true);
        }
    }
}
