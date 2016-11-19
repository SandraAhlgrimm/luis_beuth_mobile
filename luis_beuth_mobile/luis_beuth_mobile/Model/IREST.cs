using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luis_beuth_mobile.Model
{
    interface IREST<T>
    {
        Task<List<T>> Get();

        Task<T> GetById(int id);     
    }
}
