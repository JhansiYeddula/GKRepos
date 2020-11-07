using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKCustomAddress.Helper
{
    public static class CustomAddressAttributes
    {
        public static string poad_customaddressid = "poad_customaddressid";
        public static string poad_name = "poad_name";
        public static string poad_postalcode = "poad_postalcode";
        public static string poad_upszone = "poad_upszone";
        public static string poad_telephone3 = "poad_telephone3";
        public static string poad_line3 = "poad_line3";
        public static string poad_line2 = "poad_line2";
        public static string poad_line1 = "poad_line1";
        public static string poad_stateorprovince = "poad_stateorprovince";
        public static string poad_shippingmethodcode = "poad_shippingmethodcode";
        public static string poad_utcoffset = "poad_utcoffset";
        public static string poad_postofficebox = "poad_postofficebox";
        public static string poad_telephone2 = "poad_telephone2";
        public static string poad_telephone1 = "poad_telephone1";
        public static string poad_longitude = "poad_longitude";
        public static string poad_latitude = "poad_latitude";
        public static string poad_freighttermscode = "poad_freighttermscode";
        public static string poad_fax = "poad_fax";
        public static string poad_exchangerate = "poad_exchangerate";
        public static string poad_transactioncurrencyid = "poad_transactioncurrencyid";
        public static string poad_county = "poad_county";
        public static string poad_country = "poad_country";
      //  public static string poad_contactid = "poad_contactid";
        public static string poad_city = "poad_city";
        public static string poad_addresstypecode = "poad_addresstypecode";
        public static string poad_addressnumber = "poad_addressnumber";
        public static string poad_primarycontactname = "poad_primarycontactname";
        public static string poad_composite = "poad_composite";
        public static string poad_customerid = "poad_customerid";
       // public static string poad_accountid = "poad_accountid";

    }
    public static class AddressAttributes
    {
        public static string name = "name";
        public static string line1 = "line1";
        public static string city = "city";
        public static string postalcode = "postalcode";
        public static string telephone1 = "telephone1";
        public static string customeraddressid = "customeraddressid";
        public static string utcoffset = "utcoffset";
        public static string upszone = "upszone";
        public static string telephone3 = "telephone3";
        public static string line3 = "line3";
        public static string line2 = "line2";
        public static string stateorprovince = "stateorprovince";
        public static string shippingmethodcode = "shippingmethodcode";
        public static string postofficebox = "postofficebox";
        public static string telephone2 = "telephone2";
        public static string longitude = "longitude";
        public static string latitude = "latitude";
        public static string freighttermscode = "freighttermscode";
        public static string fax = "fax";
        public static string exchangerate = "exchangerate";
        public static string transactioncurrencyid = "transactioncurrencyid";
        public static string county = "county";
        public static string country = "country";
        public static string addresstypecode = "addresstypecode";
        public static string addressnumber = "addressnumber";
        public static string primarycontactname = "primarycontactname";
        public static string objecttypecode = "objecttypecode";
        public static string parentid    = "parentid";
        
    }
    public class CustomAddressEntity
    {
        public string poad_name { get; set; }
        public Guid poad_customaddressid { get; set; }
        public string poad_postalcode { get; set; }
        public string poad_upszone { get; set; }
        public string poad_telephone3 { get; set; }
        public string poad_line3 { get; set; }
        public string poad_line2 { get; set; }
        public string poad_line1 { get; set; }
        public string poad_stateorprovince { get; set; }
        public OptionSetValue poad_shippingmethodcode { get; set; }
        public int ? poad_utcoffset { get; set; }
        public string poad_postofficebox { get; set; }
        public string poad_telephone2 { get; set; }
        public string poad_telephone1 { get; set; }
        public float ? poad_longitude { get; set; }
        public float ? poad_latitude { get; set; }
        public OptionSetValue poad_freighttermscode { get; set; }
        public string poad_fax { get; set; }
        public decimal ? poad_exchangerate { get; set; }
        public EntityReference poad_transactioncurrencyid { get; set; }
        public string poad_county { get; set; }
        public string poad_country { get; set; }
        public EntityReference poad_contactid { get; set; }
        public string  Poad_city { get; set; }
        public OptionSetValue poad_addresstypecode { get; set; } //optionset
        public int poad_addressnumber { get; set; }
        public string poad_composite { get; set; }
        public EntityReference poad_accountid { get; set; }
        public string poad_primarycontactname { get; set; }


    }
}
