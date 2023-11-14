namespace EDCS_TG.API.DTO
{
    public class EducationDto
    {
        public string? EducationalQualification { get; set; }
        public bool? EducationOtherSpecify { get; set; }
        public bool? StudySchool { get; set; }
        public string? StudySchoolIfOther { get; set; }
        public int? ReceivedTrainingSkills { get; set; }

        public string? Skill { get; set; }
        public string? TrainingYears { get; set; }
        public string? WhereSkill { get; set; }
        public bool? SkillToAcquire { get; set; }
        public int? InformalEducation { get; set; }
        public bool? InformalIfYesSpecify { get; set; }
        public bool? InformalIfOthersSpecify { get; set; }
        public int? PresentlyStudying { get; set; }
        public bool? WhatClass { get; set; }
        public bool? SupportForEducation { get; set; }
        public int? HouseDependent { get; set; }
        public string? StudyingPeople { get; set; }
        public string? SupportNeeded { get; set; }
        public string? LikeToContinueEducation { get; set; }
        public string? WhichClass { get; set; }
        public string? SpeakLanguages { get; set; }

        public string? SpeakOthersSpecify { get; set; }
        public string? WriteLanguages { get; set; }
        public bool? WriteOthersSpecify { get; set; }
        public string? ReadLanguages { get; set; }
        public string? ReadOthersSpecify { get; set; }

        public string? DiscriminatedSchool { get; set; }

        public bool? PushedOutDueToIdentity { get; set; }

        public bool? SubjectedVoilence { get; set; }



    }
}
