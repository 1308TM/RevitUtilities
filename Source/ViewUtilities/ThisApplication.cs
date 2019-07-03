/*
 * Created by SharpDevelop.
 * User: Tony Martin
 * Date: 24/06/2019
 * Time: 16:50
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace ViewUtilities
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("B4D4C9D3-DE18-4F5B-AE52-D0A97E4D197D")]
	public partial class ThisApplication
	{
		private void Module_Startup(object sender, EventArgs e)
		{

		}

		private void Module_Shutdown(object sender, EventArgs e)
		{

		}

		#region Revit Macros generated code
		private void InternalStartup()
		{
			this.Startup += new System.EventHandler(Module_Startup);
			this.Shutdown += new System.EventHandler(Module_Shutdown);
		}
		#endregion
		public void ViewList()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			FilteredElementCollector viewCollector = new FilteredElementCollector(doc);
			ICollection<ElementId> collectedViews = viewCollector.OfClass(typeof(ViewPlan)).ToElementIds();
			
			string viewList = "";
                int a = 0;
                double levelElev =0;
                	
                foreach (ElementId eid in collectedViews)
                {
            
                	ViewPlan myview = doc.GetElement(eid) as ViewPlan;
                	View view = doc.GetElement(eid) as View;
                	
                	
                	
                		
                	
                	if ((myview.Name != "") && (myview.GenLevel != null) && ViewType.Equals(myview.ViewType, ViewType.FloorPlan))
                	{
                		string myViewType = myview.ViewType.ToString();
                		Level myLevel = doc.GetElement(myview.GenLevel.Id) as Level;
                		levelElev = myLevel.Elevation * 304.8;
                		viewList += a.ToString() + "_view_" + myview.Name.ToString() + "_LEVEL_" + (Math.Round(levelElev*10)/10).ToString() + "_template_" + myview.ViewTemplateId
                			+ myViewType + Environment.NewLine;
                		//uidoc.ActiveView = view;
                		a++;
                	}
                }
                TaskDialog.Show("ViewsInModel",viewList);
		
            
            
            
            
		}
		public void SheetDetails()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			FilteredElementCollector sheetCollector = new FilteredElementCollector(doc);
			ICollection<ElementId> collectedSheets = sheetCollector.OfClass(typeof(ViewSheet)).ToElementIds();
			
			string sheetList = "", RenumShts = "Renumber Sheets";
                int a = 0, currShtNo;
                
                
                foreach (ElementId eid in collectedSheets)
                {
                	ViewSheet mySheet = doc.GetElement(eid) as ViewSheet;
                	currShtNo = int.Parse(mySheet.SheetNumber) ;
                	currShtNo = currShtNo + 10000;
                	
                		Transaction transRenum = new Transaction(doc, RenumShts);
                		
                		transRenum.Start(RenumShts);
                		mySheet.SheetNumber = currShtNo.ToString();
                		transRenum.Commit();
                	
                	sheetList += a.ToString() + "____Sheet Number____" + mySheet.SheetNumber + Environment.NewLine;
                	
                	a++;
		}
                TaskDialog.Show("Sheet Numbers",sheetList);
		}
		public void ViewOnSheets()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Transaction transaction = new Transaction(doc, "View On Sheet");
			transaction.Start("View On Sheet");
			
			ElementId sheetID = new ElementId( 1316535 );
			ElementId viewID = new ElementId( 1316525 );
			
			//string myview = doc.GetElement(viewID).Name;
			//TaskDialog.Show("info", myview);
				
			Viewport.Create(doc, sheetID, viewID, new XYZ());
			
			
			transaction.Commit();
			
		}
	}
}