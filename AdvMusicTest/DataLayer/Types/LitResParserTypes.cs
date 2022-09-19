using System.Xml.Serialization;

namespace AdvMusicTest.DataLayer.Types;

[Serializable()]
public class LROffer
{
    [XmlAttribute("id")]
    public string LitresId { get; set; }
    [XmlElement("url")]
    public string Url {get; set;}
    [XmlElement("price")]
    public string Price {get; set;}
    [XmlElement("currencyId")]
    public string CurrencyId {get; set;}
    [XmlElement("categoryId")]
    public string CategoryId {get; set;}
    [XmlElement("picture")]
    public string Picture {get; set;}
    [XmlElement("author")]
    public string Author {get; set;}
    [XmlElement("name")]
    public string Name {get; set;}
    [XmlElement("publisher")]
    public string Publisher {get; set;}
    [XmlElement("series")]
    public string Series {get; set;}
    [XmlElement("year")]
    public string Year {get; set;}
    [XmlElement("ISBN")]
    public string ISBN {get; set;}
    [XmlElement("performed_by")]
    public string Performed_by {get; set;}
    [XmlElement("format")]
    public string Format {get; set;}
    [XmlElement("description")]
    public string Description {get; set;}
    [XmlElement("downloadable")]
    public string Downloadable {get; set;}
    [XmlElement("age")]
    public string Age {get; set;}
    [XmlElement("lang")]
    public string Lang {get; set;}
    [XmlElement("param")]
    public string Param {get; set;}
    [XmlElement("litres_isbn")]
    public string LitresIsbn {get; set;}
    [XmlElement("genres_list")]
    public string Genres_list {get; set;}
    
}

[Serializable()]
public class LRShop
{
    [XmlArray("offers")]
    [XmlArrayItem("offer", typeof(LROffer))]
    public LROffer[] Offers { get; set; }
}

[Serializable()]
[XmlRoot("yml_catalog")]
public class LRCatalog
{
    [XmlElement("shop")]
    public LRShop Shop { get; set; }
}