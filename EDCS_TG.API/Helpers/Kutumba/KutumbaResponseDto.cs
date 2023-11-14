namespace EDCS_TG.API.Helpers.Kutumba
{
    public class KutumbaResponseDto
    {
        public int StatusCode { get; set; }
        public string? StatusText { get; set; }
        public string? Response_ID { get; set; }
        public string? Request_ID { get; set; }
        public List<ResultDataList>? ResultDataList { get; set; }
    }
    public class ResultDataList
    {
        public string? Family_Id { get; set; }
        public string? Member_Id { get; set; }
        public string? Rc_Number { get; set; }
        public string? Member_Name_Eng { get; set; }
        public string? Mbr_Hash_Aadhar { get; set; }
        public string? Relation_Name { get; set; }
        public string? Mbr_DOB { get; set; }
        public string? Mbr_Gender { get; set; }
        public string? HasedResultValue { get; set; }

        public string? MBR_ADDRESS { get; set; }
        
    }

    public class SelectedMemberDetails
    {
        public string? FamilyId { get; set; }
        public string? MemberId { get; set; }
        public string? RCNumber { get; set; }
        public string? MemberName { get; set; }
        public string? MemberAadharHash { get; set; }
        public string? RelationName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? HashOfData { get; set; }
    }
}
