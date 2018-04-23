using DevExpress.Xpo;

using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base.General;

namespace WinSample.Module {
    [DefaultClassOptions]
    public class Item : BaseObject, ICategorizedItem {
        public Item(Session session) : base(session) { }
        private string _Name;
        public string Name {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }
        private Category _Category;
        public Category Category {
            get { return _Category;}
            set { SetPropertyValue("Category", ref _Category, value); }
        }
        #region ICategorizedItem Members
        ITreeNode ICategorizedItem.Category {
            get {
                return Category;
            }
            set {
                Category = (Category)value;
            }
        }
        #endregion
    }
}
