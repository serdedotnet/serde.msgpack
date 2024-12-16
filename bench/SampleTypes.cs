
#nullable disable

using System;
using MessagePack;
using Serde;

namespace Benchmarks
{
    // the view models come from a real world app called "AllReady"
    [GenerateSerialize, GenerateDeserialize]
    public partial class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    [GenerateSerialize, GenerateDeserialize]
    [MessagePackObject]
    public partial record Location
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Address1 { get; set; }
        [Key(2)]
        public string Address2 { get; set; }
        [Key(3)]
        public string City { get; set; }
        [Key(4)]
        public string State { get; set; }
        [Key(5)]
        public string PostalCode { get; set; }
        [Key(6)]
        public string Name { get; set; }
        [Key(7)]
        public string PhoneNumber { get; set; }
        [Key(8)]
        public string Country { get; set; }

        public static Location Sample => new Location
        {
            Id = 1234,
            Address1 = "The Street Name",
            Address2 = "20/11",
            City = "The City",
            State = "The State",
            PostalCode = "abc-12",
            Name = "Nonexisting",
            PhoneNumber = "+0 11 222 333 44",
            Country = "The Greatest"
        };
    }

    public sealed partial class LocationWrap : IDeserialize<Location>, IDeserializeProvider<Location>
    {
        public static LocationWrap Instance { get; } = new();
        static IDeserialize<Location> IDeserializeProvider<Location>.DeserializeInstance => Instance;
        private LocationWrap() { }

        public static ISerdeInfo SerdeInfo { get; } = Serde.SerdeInfo.MakeCustom(
            "Location",
            typeof(Location).GetCustomAttributesData(),
            [
                ("id", Int32Proxy.SerdeInfo, typeof(Location).GetProperty("Id")!),
                ("address1", StringProxy.SerdeInfo, typeof(Location).GetProperty("Address1")!),
                ("address2", StringProxy.SerdeInfo, typeof(Location).GetProperty("Address2")!),
                ("city", StringProxy.SerdeInfo, typeof(Location).GetProperty("City")!),
                ("state", StringProxy.SerdeInfo, typeof(Location).GetProperty("State")!),
                ("postalCode", StringProxy.SerdeInfo, typeof(Location).GetProperty("PostalCode")!),
                ("name", StringProxy.SerdeInfo, typeof(Location).GetProperty("Name")!),
                ("phoneNumber", StringProxy.SerdeInfo, typeof(Location).GetProperty("PhoneNumber")!),
                ("country", StringProxy.SerdeInfo, typeof(Location).GetProperty("Country")!)
            ]);

        Benchmarks.Location Serde.IDeserialize<Benchmarks.Location>.Deserialize(IDeserializer deserializer)
        {
            int _l_id = default !;
            string _l_address1 = default !;
            string _l_address2 = default !;
            string _l_city = default !;
            string _l_state = default !;
            string _l_postalcode = default !;
            string _l_name = default !;
            string _l_phonenumber = default !;
            string _l_country = default !;
            ushort _r_assignedValid = 0b0;

            var _l_serdeInfo = SerdeInfo;
            var typeDeserialize = deserializer.ReadType(_l_serdeInfo);
            int index;
            while ((index = typeDeserialize.TryReadIndex(_l_serdeInfo, out _)) != IDeserializeType.EndOfType)
            {
                switch (index)
                {
                    case 0:
                        _l_id = typeDeserialize.ReadI32(index);
                        _r_assignedValid |= ((ushort)1) << 0;
                        break;
                    case 1:
                        _l_address1 = typeDeserialize.ReadString(index);
                        _r_assignedValid |= ((ushort)1) << 1;
                        break;
                    case 2:
                        _l_address2 = typeDeserialize.ReadString(index);
                        _r_assignedValid |= ((ushort)1) << 2;
                        break;
                    case 3:
                        _l_city = typeDeserialize.ReadString(index);
                        _r_assignedValid |= ((ushort)1) << 3;
                        break;
                    case 4:
                        _l_state = typeDeserialize.ReadString(index);
                        _r_assignedValid |= ((ushort)1) << 4;
                        break;
                    case 5:
                        _l_postalcode = typeDeserialize.ReadString(index);
                        _r_assignedValid |= ((ushort)1) << 5;
                        break;
                    case 6:
                        _l_name = typeDeserialize.ReadString(index);
                        _r_assignedValid |= ((ushort)1) << 6;
                        break;
                    case 7:
                        _l_phonenumber = typeDeserialize.ReadString(index);
                        _r_assignedValid |= ((ushort)1) << 7;
                        break;
                    case 8:
                        _l_country = typeDeserialize.ReadString(index);
                        _r_assignedValid |= ((ushort)1) << 8;
                        break;
                    case Serde.IDeserializeType.IndexNotFound:
                        typeDeserialize.SkipValue();
                        break;
                    default:
                        throw new InvalidOperationException("Unexpected index: " + index);
                }
            }

            if (_r_assignedValid != 0b111111111)
            {
                throw Serde.DeserializeException.UnassignedMember();
            }

            var newType = new Benchmarks.Location()
            {
                Id = _l_id,
                Address1 = _l_address1,
                Address2 = _l_address2,
                City = _l_city,
                State = _l_state,
                PostalCode = _l_postalcode,
                Name = _l_name,
                PhoneNumber = _l_phonenumber,
                Country = _l_country,
            };
            return newType;
        }
    }
}