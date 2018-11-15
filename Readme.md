<!-- default file list -->
*Files to look at*:

* **[MyCategorizedListEditor.cs](./CS/WinSample.Module.Win/MyCategorizedListEditor.cs) (VB: [MyCategorizedListEditor.vb](./VB/WinSample.Module.Win/MyCategorizedListEditor.vb))**
* [Category.cs](./CS/WinSample.Module/Category.cs) (VB: [Category.vb](./VB/WinSample.Module/Category.vb))
* [Item.cs](./CS/WinSample.Module/Item.cs) (VB: [Item.vb](./VB/WinSample.Module/Item.vb))
<!-- default file list end -->
# CategorizedListEditor - How to show subcategory items when a parent category is selected


<p>The CategorizedListEditor consist of two views - the tree list of the categories in the left area of the editor (CategoriesListView), and the list of categorized items on the right (GridView). Items in the GridView are filtered by category, selected in the CategoriesListView. If the selected category has children, items belonging to them won't be shown in the GridView. This example demonstrates, how to change this behavior, and filter GridView to show all items of the selected category and its children. For this purpose, a CategorizedListEditor's descendant - MyCategorizedListEditor - is created. It handles the SelectionChanged event of the ListView of categories and changes the criteria applied to the categorized item ListView to include items related to the nested categories.</p><p><strong>See Also:</strong><br />
<a href="http://documentation.devexpress.com/#Xaf/CustomDocument2838"><u>Implement the ICategorizedItem Interface</u></a><br />
<a href="http://documentation.devexpress.com/#Xaf/CustomDocument2839"><u>Use the Built-in HCategory Class</u></a><br />
<a href="http://documentation.devexpress.com/#Xaf/CustomDocument2659"><u>How to: Implement a Custom Windows Forms List Editor</u></a></p>

<br/>


