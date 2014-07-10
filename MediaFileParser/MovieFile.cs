#region



#endregion

namespace MediaFileParser
{
    public class MovieFile : MediaFile
    {
        public MovieFile(string file) : base(file)
        {
            // No further parsing required.
        }

        public override string Cleaned
        {
            get
            {
                var result = "";
                for (var i = 0; i < SectorList.Count; i++)
                {
                    if (SectorList[i] == "-") continue;
                    result += SectorList[i] + " ";
                }
                result = result.Trim();
                return result;
            }
        }

        public override bool Test()
        {
            return true;
        }
    }
}