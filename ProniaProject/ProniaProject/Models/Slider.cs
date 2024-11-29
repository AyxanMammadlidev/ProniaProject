using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ProniaProject.Models
{
    public class Slider : BaseEntity
    {

        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Desc { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }





    }
}
