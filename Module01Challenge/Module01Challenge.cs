using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Module01Challenge
{

    [Transaction(TransactionMode.Manual)]
    public class Module01Challenge : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;

            int currentLevel = 0;


            //sheets collector
            FilteredElementCollector tbCollector = new FilteredElementCollector(doc)
            .OfCategory(BuiltInCategory.OST_TitleBlocks)
            .WhereElementIsElementType();
            ElementId tb = tbCollector.FirstElementId();

            //view plan
            FilteredElementCollector vftCollector = new FilteredElementCollector(doc);
            vftCollector.OfClass(typeof(ViewFamilyType));

            ViewFamilyType fpvft = null;
            ViewFamilyType cpvft = null;


            foreach (ViewFamilyType curvft in vftCollector)
            {
                if (curvft.ViewFamily == ViewFamily.FloorPlan) { fpvft = curvft; }

                else if (curvft.ViewFamily == ViewFamily.CeilingPlan) { cpvft = curvft; }
            }



            Transaction t = new Transaction(doc);

            t.Start("do levels");

            for (var i = 1; i <= 250; i++)
            {
                // Create Level
                Level newlvl = Level.Create(doc, currentLevel);
                newlvl.Name = "Level " + i.ToString();

                // increment the level elevation 
                currentLevel += 15;


                if (i % 3 == 0 && i % 5 == 0)
                {
                    //fizzbuzz create sheets
                    ViewSheet shee = ViewSheet.Create(doc, tb);
                    shee.Name = "fizzBuzz__" + i.ToString();
                    shee.SheetNumber = i.ToString();
   
                }
                else if (i % 3 == 0)
                {
                    ViewPlan newPlan = ViewPlan.Create(doc, fpvft.Id , newlvl.Id);
                    newPlan.Name = $"Fizz__{i.ToString()}";

                   
                }
                else if (i % 5 == 0)
                {
                    ViewPlan newCeilingPlan = ViewPlan.Create(doc, cpvft.Id, newlvl.Id);
                    newCeilingPlan.Name = $"Buzz__{i.ToString()}";

                    //what is the difference between the next lind and the above line
                    //Ceiling newCei = Ceiling.Create(doc,,cpvft.Id, newlvl.Id);

                }

            }


            t.Commit();
            t.Dispose();
            

            TaskDialog.Show("complete", "created " + 250 + " Levels");

            return Result.Succeeded;


        }
    }
}
