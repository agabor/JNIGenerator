namespace JNIGenerator
{
    public class LType
    {
        public string Sourcename { get; set; }
        public string Targetname { get; set; }
        public bool Isprimitive => !Iscustom;
        public bool Iscustom => Isstruct || Isenum;
        public bool Isstruct { get; set; }
        public bool Isenum { get; set; }
        public bool Isarray { get; set; }

        internal LType Clone()
        {
            return new LType
            {
                Sourcename = Sourcename,
                Targetname = Targetname,
                Isenum = Isenum,
                Isstruct = Isstruct,
                Isarray = Isarray
            };
        }
    }
}