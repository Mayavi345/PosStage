using System.ComponentModel.DataAnnotations;

namespace Stage.Data.Models.DataModel
{
    public class MB1001_Member
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public string PhoneNumber { get; set; }
        public int MemberLevel { get; set; }
        public bool IsDelete { get; set; }

        public void MapMB1001_MemberData(MB1001_Member newItem) {
            Name = newItem.Name;
            Gender = newItem.Gender;
            PhoneNumber = newItem.PhoneNumber;
            MemberLevel = newItem.MemberLevel;
            IsDelete = newItem.IsDelete;
        }
    }
}
