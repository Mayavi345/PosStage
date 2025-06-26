using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class TextResourceCenter
    {
        public const string FailText = "Fail";
        public const string WarmTextEmoji = "⚠";

        public const string OrderSuccessText = "購買成功，訂單號碼:";
        public const string OrderFailText = "購買失敗:";
        public const string OrderFailNotHaveProductText = "購買失敗:.購物車沒東西:";

        public const string FindNotSettingJsonFile = "找不到Setting.json，系統已自動生成";
        public const string CreateMemberSuccess = "成功建立會員:";
        public const string CreateMemberFail = "建立會員失敗，請稍後再嘗試:";
        public const string MemberDataIsNull = "無該會員資料";
        public const string ConfirmIsDelete = "是否要刪除";
        public const string DataIsNotAllowNull = "產品資料不能為空";

        public const string FiledValidateFail = "欄位驗證失敗";
        public const string AddSuccess = "新增成功";
        public const string SystemHaveThisMember = "系統中有該員工";

        public const string RequiredText_InputCategoryText = "請輸入類別名稱";

    }
}
