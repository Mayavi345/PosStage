using Stage.Data.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PosStage.MVVM.Models;

namespace Stage.Data.Models
{
    public class MemberModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GenderModel Gender { get; set; }
        public string PhoneNumber { get; set; }

        public MB1002_ConsumptionLevelRule ConsumptionLevel { get; set; }
        public MB1001_Member ConvertToMember()
        {
            MB1001_Member member = new MB1001_Member();
            member.Id = Id;
            member.Name = Name;
            member.Gender = Gender.Id;
            member.PhoneNumber = PhoneNumber;
            member.MemberLevel = ConsumptionLevel.MemberLevel;
            member.IsDelete = false;
            return member;
        }
        public MemberModel() { }
        public MemberModel(MemberModel memberModel)
        {
            Id = memberModel.Id;
            Name = memberModel.Name;
            Gender = memberModel.Gender;
            PhoneNumber = memberModel.PhoneNumber;
            ConsumptionLevel = memberModel.ConsumptionLevel;
        }
        public static GenderModel GetGender(int id)
        {
            GenderModel gender = new GenderModel();
            if (id == 1)
            {
                gender.Id = 1;
                gender.Name = "男性";
            }
            else if (id == 2)
            {
                gender.Id = 2;
                gender.Name = "女性";
            }
            return gender;
        }
    }
}
