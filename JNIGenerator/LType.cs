namespace JNIGenerator
{
    public class LType
    {
        public string Sourcename { get; set; }
        public string Targetname { get; set; }
        public bool Isprimitive { get; set; }
        public bool Iscustom { get; set; }
        public bool Isarray { get; set; }

        internal LType Clone()
        {
            return new LType
            {
                Sourcename = Sourcename,
                Targetname = Targetname,
                Isprimitive = Isprimitive,
                Iscustom = Iscustom,
                Isarray = Isarray
            };
        }
    }
}