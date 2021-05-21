/* *
 * Abril media player
 * 
 * Programmer: Diogo Rodrigues Roessler - SOOAHPAZ ( 5/20/2021 )
 * 
 * 
 * 
 * */


namespace System.Windows
{
    [Localizability(LocalizationCategory.None, Readability = Readability.Inherit)]
    public enum flow_diration
    {
        LeftToRight = 0,
        RightToLeft = 1,
        TopDown = 2,
        BottomUp = 3
    }

    public class Framework_Element
    {
        [Localizability(LocalizationCategory.None)]
#pragma warning disable CS0436 // Type conflicts with imported type
        public flow_diration flow_diration { get; set; }
#pragma warning restore CS0436 // Type conflicts with imported type
    }
}
