using Stage.Presentation.Common;
using UIComponent.Page;

namespace Stage.BLL
{
    public interface IPageHelper
    {
        PageObject GetPageObject(EViewPage eViewPage);
    }
}