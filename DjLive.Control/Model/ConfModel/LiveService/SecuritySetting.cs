namespace DjLive.Control.Model.ConfModel.LiveService
{
    public class SecuritySetting
    {
        /// <summary>
        /// allow -- deny
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// publish---play
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// all --- ip
        /// </summary>
        public string Ip { get; set; }
    }
}