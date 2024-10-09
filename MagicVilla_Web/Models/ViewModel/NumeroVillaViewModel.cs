using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModel
{
	public class NumeroVillaViewModel
	{
        public NumeroVillaCreateDto NumeroVilla { get; set; }
        public IEnumerable<SelectListItem> VillaList { get; set; }

        public NumeroVillaViewModel()
        {
            NumeroVilla = new NumeroVillaCreateDto();
        }

    }
}
