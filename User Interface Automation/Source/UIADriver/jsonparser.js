import System.Reflection;
[assembly:AssemblyKeyFileAttribute("uiadriver.snk")]

public class jsonparser 
{
	function ParseObj(injson, field) 
	{
	   var obj = eval ("(" + injson + ")");

	   return eval("obj."+field);     
	}
}

