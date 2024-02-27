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




            Transaction t = new Transaction(doc);

            t.Start("do");

            for (var i = 1; i < 250; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    Level lv = Level.Create(doc, i);
                    lv.Name = $"FizzBuzz {i}";
                   
                }
                else if (i % 3 == 0)
                {
                    Level lv = Level.Create(doc, i);
                    lv.Name = $"Fizz {i}";
                    
                }
                else if (i % 5 == 0)
                {
                    Level lv = Level.Create(doc, i);
                    lv.Name = $"Buzz {i}";
                   

                }
            }



            t.Commit();

            return Result.Succeeded;


        }
    }
}