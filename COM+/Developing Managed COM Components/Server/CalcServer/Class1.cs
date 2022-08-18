using System;
using System.Runtime.InteropServices;
using System.EnterpriseServices;
namespace CalcServer
{
    [ComVisible(true)]
    [Guid("E3D02A14-ED36-471E-BE32-2D65F1BC97DA")]
    public interface ICalucalator
    {
        int add(int op1, int op2);
    }

    [ComVisible(true)]
    [Guid("0AD70785-5539-4EE0-83D5-37A62CF5318F")]
    public class Calucalator : ServicedComponent,  ICalucalator
    {

        public int add(int op1, int op2)
        {
            return op1 + op2;
        }
    }
 
}
