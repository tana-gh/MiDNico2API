using System;
using System.Runtime.Serialization;
using System.Net;
using System.Linq;

namespace MiDNicoAPI.Entity
{
    [DataContract(Name = "nicovideo_user_response", Namespace = "")]
    public sealed class UserInfo
    {
        internal UserInfo(in string id, in string name, in string thumbUrl, in int userSecret, in object additionals)
        {
            this.User        = new UserEntity(id, name, thumbUrl);
            this.VitaOption  = new VitaOptionEntity(userSecret);
            this.Additionals = additionals;
        }

        [DataMember(Name = "user"       , Order = 0)] public UserEntity       User        { get; private set; }
        [DataMember(Name = "vita_option", Order = 1)] public VitaOptionEntity VitaOption  { get; private set; }
        [DataMember(Name = "additionals", Order = 2)] public object           Additionals { get; private set; }

        [DataContract(Name = "user", Namespace = "")]
        public sealed class UserEntity
        {
            internal UserEntity(in string id, in string name, in string thumbUrl)
            {
                this.ID           = id;
                this.NickName     = name;
                this.ThumbnailUrl = thumbUrl;
            }

            [DataMember(Name = "id"           , Order = 0)] public string ID           { get; private set; }
            [DataMember(Name = "nickname"     , Order = 1)] public string NickName     { get; private set; }
            [DataMember(Name = "thumbnail_url", Order = 2)] public string ThumbnailUrl { get; private set; }
        }

        [DataContract(Name = "vita_option", Namespace = "")]
        public sealed class VitaOptionEntity
        {
            internal VitaOptionEntity(in int userSecret)
            {
                this.UserSecret = userSecret;
            }

            [DataMember(Name = "user_secret", Order = 0)] public int UserSecret { get; private set; }
        }
    }
}
