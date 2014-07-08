namespace MediaFileParser
{
    public class MovieFile : MediaFile
    {
        public MovieFile(string file) : base(file)
        {
            if (SectorList.Count <= 1) return;

            // Attempt to remove year made
            int num;
            if (int.TryParse(SectorList[SectorList.Count - 1], out num) && num > 1500 && num < 3000)
            {
                SectorList.RemoveAt(SectorList.Count - 1);
            }
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
    }
}