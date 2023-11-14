using System.Threading;
using System.Threading.Tasks;

using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmissionBasicDetails;
using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Dto.Enquiry;
using KAR.KSFC.Components.Common.Utilities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KAR.KSFC.API.Areas.Customer.Controllers
{
    public class EnquirySubmissionBasicDetailsController : BaseApiController
    {
        private readonly UserInfo _userInfo;
        private readonly ILogger<EnquirySubmissionBasicDetailsController> _logger;
        private readonly IBasicDetails _basicDetails;
        private readonly IPromoterGuarantorDetails _promoterGuarantorDetails;
        private readonly IAssociateSisterConcern _associateSisterConcern;
        public EnquirySubmissionBasicDetailsController(UserInfo userInfo, ILogger<EnquirySubmissionBasicDetailsController> logger, 
            IBasicDetails basicDetails, IPromoterGuarantorDetails promoterGuarantorDetails, IAssociateSisterConcern associateSisterConcern)
        {
            _userInfo = userInfo;
            _logger = logger;
            _basicDetails = basicDetails;
            _promoterGuarantorDetails = promoterGuarantorDetails;
            _associateSisterConcern = associateSisterConcern;
        }

        #region Start Personal Details
        [HttpGet, Route("GetAllPersonalDetailsList")]
        public async Task<IActionResult> GetAllPersonalDetailsList(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeletePersonalDetails")]
        public async Task<IActionResult> DeletePersonalDetails([FromRoute]int PersonalId, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddPerosnalDetail")]
        public async Task<IActionResult> AddPerosnalDetail([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _basicDetails.AddPersonalDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdatePerosnalDetail")]
        public async Task<IActionResult> UpdatePerosnalDetailAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
           //  var lst = await _basicDetails.UpdatePersonalDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdPerosnalDetail")]
        public async Task<IActionResult> GetByIdPerosnalDetailAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {          
            return Ok();
        }
        #endregion

        #region Start Address Details crud
        [HttpGet, Route("GetAllAddressDetailsList")]
        public async Task<IActionResult> GetAllAddressDetailsList(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeleteAddressDetails")]
        public async Task<IActionResult> DeleteAddress([FromRoute] int PersonalId, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddAddressDetail")]
        public async Task<IActionResult> AddAddressDetail([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _basicDetails.AddAddressDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdateAddressDetail")]
        public async Task<IActionResult> UpdateAddressDetailAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
             //var lst = await _basicDetails.UpdateAddressDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdAddressDetail")]
        public async Task<IActionResult> GetByIdAddressDetailAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion

        #region Start Bank Details crud
        [HttpGet, Route("GetAllBankDetailsList")]
        public async Task<IActionResult> GetAllBankDetailsList(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeleteBankDetails")]
        public async Task<IActionResult> DeleteBankDetail([FromRoute] int PersonalId, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddBankDetail")]
        public async Task<IActionResult> AddBankDetail([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _basicDetails.AddBankDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdateBankDetail")]
        public async Task<IActionResult> UpdateBankDetailAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _basicDetails.UpdateBankDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdBankDetail")]
        public async Task<IActionResult> GetByIdBankDetailAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion

        #region Start Registration Details crud
        [HttpGet, Route("GetAllRegistrationDetailsList")]
        public async Task<IActionResult> GetAllRegistrationDetailsList(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeleteRegistrationDetails")]
        public async Task<IActionResult> DeleteRegistrationDetail([FromRoute] int PersonalId, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddRegistrationDetail")]
        public async Task<IActionResult> AddRegistrationDetail([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _basicDetails.AddRegistrationDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdateRegistrationDetail")]
        public async Task<IActionResult> UpdateRegistrationDetailAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _basicDetails.UpdateRegistrationDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdRegistrationDetail")]
        public async Task<IActionResult> GetByIdRegistrationDetailAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion

        #region Start Promoter and Guarantor Details Tab crud
        [HttpGet, Route("GetPromoterDetailsList")]
        public async Task<IActionResult> GetPromoterDetailsList(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeletePromoterDetails")]
        public async Task<IActionResult> DeletePromoterDetails([FromRoute] int PromoterId, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddPromoterDetails")]
        public async Task<IActionResult> AddPromoterDetails([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _promoterGuarantorDetails.AddPromoterDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdatePromoterDetails")]
        public async Task<IActionResult> UpdatePromoterDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
           // var lst = await _promoterGuarantorDetails.UpdatePromoterDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdPromoterDetails")]
        public async Task<IActionResult> GetByIdPromoterDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion

        #region Start Promoter Net Worth Dtails Tab crud
        [HttpGet, Route("GetPromoterNetWorthDetailsList")]
        public async Task<IActionResult> GetPromoterNetWorthDetailsList(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeletePromoterNetWorthDetails")]
        public async Task<IActionResult> DeletePromoterNetWorthDetails([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddPromoterNetWorthDetails")]
        public async Task<IActionResult> AddPromoterNetWorthDetails([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _promoterGuarantorDetails.AddPromoterNetWorthDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdatePromoterNetWorthDetails")]
        public async Task<IActionResult> UpdatePromoterNetWorthDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            // var lst = await _promoterGuarantorDetails.UpdatePromoterNetWorthDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdPromoterNetWorthDetails")]
        public async Task<IActionResult> GetByIdPromoterNetWorthDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion

        #region Start Promoter Liability Dtails Tab crud
        [HttpGet, Route("GetPromoterLiabilityDetailsList")]
        public async Task<IActionResult> GetPromoterLiabilityDetailsListAsync(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeletePromoterLiabilityDetails")]
        public async Task<IActionResult> DeletePromoterLiabilityDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddPromoterLiabilityDetails")]
        public async Task<IActionResult> AddPromoterLiabilityDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _promoterGuarantorDetails.AddPromoterLiabilityDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdatePromoterLiabilityDetails")]
        public async Task<IActionResult> UpdatePromoterNetLiabilityDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            // var lst = await _promoterGuarantorDetails.UpdatePromoterLiabilityDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdPromoterLibilityDetails")]
        public async Task<IActionResult> GetByIdPromoterLiabilityDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion

        #region Start Promoter Liability Net Worth Dtails Tab crud
        [HttpGet, Route("GetPromoterLiabilityNetWorthList")]
        public async Task<IActionResult> GetPromoterLiabilityNetWorthDetailsListAsync(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeletePromoterLiabilityNetWorth")]
        public async Task<IActionResult> DeletePromoterLiabilityNetWorthDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddPromoterLiabilityNetWorth")]
        public async Task<IActionResult> AddPromoterLiabilityNetWorthDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _promoterGuarantorDetails.AddPromoterLiabilityNetWorthDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdatePromoterLiabilityNetWorth")]
        public async Task<IActionResult> UpdatePromoterNetLiabilityNetWorthDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
             //var lst = await _promoterGuarantorDetails.UpdatePromoterLiabilityNetWorthDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdPromoterLibilityNetWorth")]
        public async Task<IActionResult> GetByIdPromoterLiabilityNetWorthDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion

        #region Start Promoter and Guarantor Details Tab crud
        [HttpGet, Route("GetGuarantorDetailsList")]
        public async Task<IActionResult> GetGuarantorDetailsList(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeleteGuarantorDetails")]
        public async Task<IActionResult> DeleteGuarantorDetails([FromRoute] int GuarantorId, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddGuarantorDetails")]
        public async Task<IActionResult> AddGuarantorDetails([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _promoterGuarantorDetails.AddGuarantorDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdateGuarantorDetails")]
        public async Task<IActionResult> UpdateGuarantorDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            // var lst = await _promoterGuarantorDetails.UpdateGuarantorDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdGuarantorDetails")]
        public async Task<IActionResult> GetByIdGuarantorDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion

        #region Start Guarantor Net Worth Dtails Tab crud
        [HttpGet, Route("GetGuarantorNetWorthDetailsList")]
        public async Task<IActionResult> GetGuarantorNetWorthDetailsList(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeleteGuarantorNetWorthDetails")]
        public async Task<IActionResult> DeleteGuarantorNetWorthDetails([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddGuarantorNetWorthDetails")]
        public async Task<IActionResult> AddGuarantorNetWorthDetails([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _promoterGuarantorDetails.AddGuarantorNetWorthDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdateGuarantorNetWorthDetails")]
        public async Task<IActionResult> UpdateGuarantorNetWorthDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            // var lst = await _promoterGuarantorDetails.UpdateGuarantorNetWorthDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdGuarantorNetWorthDetails")]
        public async Task<IActionResult> GetByIdGuarantorNetWorthDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion

        #region Start Guarantor Liability Dtails Tab crud
        [HttpGet, Route("GetGuarantorLiabilityDetailsList")]
        public async Task<IActionResult> GetGuarantorLiabilityDetailsListAsync(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeleteGuarantorLiabilityDetails")]
        public async Task<IActionResult> DeleteGuarantorLiabilityDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddGuarantorLiabilityDetails")]
        public async Task<IActionResult> AddGuarantorLiabilityDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _promoterGuarantorDetails.AddGuarantorLiabilityDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdateGuarantorLiabilityDetails")]
        public async Task<IActionResult> UpdateGuarantorNetLiabilityDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            // var lst = await _promoterGuarantorDetails.UpdateGuarantorLiabilityDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdGuarantorLibilityDetails")]
        public async Task<IActionResult> GetByIdGuarantorLiabilityDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion

        #region Start Guarantor Liability Net Worth Dtails Tab crud
        [HttpGet, Route("GetGuarantorLiabilityNetWorthList")]
        public async Task<IActionResult> GetGuarantorLiabilityNetWorthDetailsListAsync(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeleteGuarantorLiabilityNetWorth")]
        public async Task<IActionResult> DeleteGuarantorLiabilityNetWorthDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddGuarantorLiabilityNetWorth")]
        public async Task<IActionResult> AddGuarantorLiabilityNetWorthDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _promoterGuarantorDetails.AddGuarantorLiabilityNetWorthDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdateGuarantorLiabilityNetWorth")]
        public async Task<IActionResult> UpdateGuarantorLiabilityNetWorthDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _promoterGuarantorDetails.UpdateGuarantorLiabilityNetWorthDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdGuarantorLibilityNetWorth")]
        public async Task<IActionResult> GetByIdGuarantorLiabilityNetWorthDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion

        #region Start Associate / Sister Concern Dtails Tab crud
        [HttpGet, Route("GetAssociateSisterConcernList")]
        public async Task<IActionResult> GetAssociateSisterConcernDetailsListAsync(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete, Route("DeleteAssociateSisterConcern")]
        public async Task<IActionResult> DeleteAssociateSisterConcernDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddAssociateSisterConcern")]
        public async Task<IActionResult> AddAssociateSisterConcernDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _associateSisterConcern.AddAssociateSisterConcernDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdateAssociateSisterConcern")]
        public async Task<IActionResult> UpdateAssociateSisterConcernDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _associateSisterConcern.UpdateAssociateSisterConcernDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdGuarantorLibilityNetWorth")]
        public async Task<IActionResult> GetByIdAssociateSisterConcernDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion

        #region Start Associate Previous Year Financial Dtails(Minimum of 3 years) Tab crud
        [HttpGet, Route("GetAssociatePreviousYearList")]
        public async Task<IActionResult> GetAssociatePreviousYearDetailsListAsync(CancellationToken cancellationToken)
        {

           

            return Ok();
        }

        [HttpDelete, Route("DeleteAssociatePreviousYear")]
        public async Task<IActionResult> DeleteAssociatePreviousYearDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        [HttpPost, Route("AddAssociatePreviousYear")]
        public async Task<IActionResult> AddAssociatePreviousYearDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _associateSisterConcern.AddAssociatePreviousYearDetailsAsync(model, cancellationToken).ConfigureAwait(false);
            return Ok();
        }
        [HttpPut, Route("UpdateAssociatePreviousYear")]
        public async Task<IActionResult> UpdateAssociatePreviousYearDetailsAsync([FromBody] BasicDetailsDTO model, CancellationToken cancellationToken)
        {
            //var lst = await _associateSisterConcern.UpdateAssociatePreviousYearDetailsAsync(model, cancellationToken).ConfigureAwait(false);

            return Ok();
        }
        [HttpGet, Route("GetByIdGuarantorLibilityNetWorth")]
        public async Task<IActionResult> GetByIdAssociatePreviousYearDetailsAsync([FromRoute] int Id, CancellationToken cancellationToken)
        {
            return Ok();
        }
        #endregion
    }
}
