using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Circular
{
    [XmlRoot(ElementName = "direto", Namespace = "")]
    public class Direto
    {
        [XmlElement(ElementName = "String", Namespace = "")]
        public List<String> hora;
    }

    [XmlRoot(ElementName = "inverso", Namespace = "")]
    public class Inverso
    {
        [XmlElement(ElementName = "String", Namespace = "")]
        public List<String> hora;
    }

    [XmlRoot(ElementName = "dia", Namespace = "")]
    public class Dia
    {
        [XmlElement(ElementName = "nome", Namespace = "")]
        public String nome;
        [XmlElement(ElementName = "direto", Namespace = "")]
        public Direto direto;
        [XmlElement(ElementName = "inverso", Namespace = "")]
        public Inverso inverso;
    }

    [XmlRoot(ElementName = "horarios", Namespace = "")]
    public class Horarios
    {
        [XmlElement(ElementName = "dia", Namespace = "")]
        public List<Dia> dia;


    }
}
