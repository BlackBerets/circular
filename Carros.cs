using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Circular
{
    [XmlRoot(ElementName = "Saidas", Namespace = "")]
    public class Saidas
    {
        [XmlElement(ElementName = "Saida", Namespace = "")]
        public List<String> Saida;
    }

    [XmlRoot(ElementName = "Carro", Namespace = "")]
    public class Carro
    {
        [XmlElement(ElementName = "ID", Namespace = "")]
        public String ID;
        [XmlElement(ElementName = "Empresa", Namespace = "")]
        public String Empresa;
        [XmlElement(ElementName = "Circuito", Namespace = "")]
        public String Circuito;
        [XmlElement(ElementName = "Saidas", Namespace = "")]
        public Saidas Saidas;
    }

    [XmlRoot(ElementName = "Carros", Namespace = "")]
    public class Carros
    {
        [XmlElement(ElementName = "Carro", Namespace = "")]
        public List<Carro> Carro;
    }


}
