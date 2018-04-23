using DevExpress.Xpo;

using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;

namespace WinSample.Module {
    [DefaultClassOptions]
    public class Category : HCategory {
        public Category(Session session) : base(session) { }
    }

}