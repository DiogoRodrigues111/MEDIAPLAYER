/* *
 * Abril media player
 * 
 * Programmer: Diogo Rodrigues Roessler - SOOAHPAZ ( 5/20/2021 )
 * 
 * 
 * 
 * */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDIA_PLAYER
{
	public static class FILE_LINE
	{
#if DEBUG
		public static String __GET_LINE__
		{
			get
			{
				StackFrame f = new StackFrame(1, true);
				return "FILE: " + f.GetFileName() + " \n " + "LINE: " + f.GetFileLineNumber() + " \n " + "COLUMS: " + f.GetFileColumnNumber() + " \n " + "ROUTINE: " + f.GetMethod();
			}
		}
#endif
	}
}
