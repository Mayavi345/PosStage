using System;

namespace Utilities
{
    public interface IFilter<T>
    {
        bool ContainsFilter(object obj);
        bool NotContainsFilter(object obj);

        void UpdateFilterText(string textToFilter);
    }
    public interface IFilterableObject
    {
        string FilterValue { get; }
    }
    public class FilterTextHelper : IFilter<IFilterableObject>
    {
        // 定義篩選條件：等於、不等於、包含、不包含


        private string _filterText;

        public FilterTextHelper(string textToFilter)
        {
            _filterText = textToFilter;
        }
        public void UpdateFilterText(string textToFilter)
        {
            _filterText = textToFilter;
        }
        bool IFilter<IFilterableObject>.ContainsFilter(object obj)
        {
            IFilterableObject text = obj as IFilterableObject;
            return text != null && text.FilterValue.Contains(_filterText);
        }
        bool IFilter<IFilterableObject>.NotContainsFilter(object obj)
        {
            IFilterableObject text = obj as IFilterableObject;
            return text != null && !text.FilterValue.Contains(_filterText);
        }

        void IFilter<IFilterableObject>.UpdateFilterText(string textToFilter)
        {
            UpdateFilterText(textToFilter);
        }

        private bool EqualsFilterAction(object obj)
        {
            throw new NotImplementedException();

        }



        private bool NotEqualsFilterAction(object obj)
        {
            throw new NotImplementedException();

        }


    }

}
