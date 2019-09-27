using ActioBP.General.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ActioBP.General.HttpModels
{
    public class CollectionPerEmpresa<T>
    {
        public string EmpresaFacturacion { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
    public class CollectionsPerEmpresa<T>
    {
        public IEnumerable<string> Empresas{
            get {
                if (this.List == null) return new List<string>();

                return this.List.Select(o => o.EmpresaFacturacion).Distinct();
            }
        }
        public ICollection<CollectionPerEmpresa<T>> List{get;set;}

        public CollectionsPerEmpresa()
        {
            this.List = new List<CollectionPerEmpresa<T>>();
        }
        public void  Add(CollectionPerEmpresa<T> item)
        {
            this.List.Add(item);
        }
        public void Add(string empresa, IEnumerable<T> Items)
        {
            this.Add(new CollectionPerEmpresa<T>
            {
                EmpresaFacturacion = empresa,
                Items = Items
            });
        }
        public void Add(string empresa, T Item)
        {
            this.Add(new CollectionPerEmpresa<T> {
                EmpresaFacturacion = empresa,
                Items = new Collection<T> {  Item }
            });
        }

        public CollectionsPerEmpresa<T> UnifyPerEmpresa(bool asignToThis)
        {
            var per = UnifyPerEmpresa(this);
            if (asignToThis) {
                this.List = per.List;
            }
            return per;
        }
        public CollectionsPerEmpresa<T> UnifyPerEmpresa(CollectionsPerEmpresa<T> oldList)
        {
            var newList = new CollectionsPerEmpresa<T> ();
            var newCol = new CollectionsPerEmpresa<T>();
            foreach (var l in oldList.List)
            {
                var val = newList.Find(l.EmpresaFacturacion);
                if (val != null)
                {
                    var val2=val.Items.ToList();
                    val2.AddRange(l.Items);
                    val.Items = val2;
                }
                else
                {
                    newList.Add(l.EmpresaFacturacion, l.Items);
                }
            }
            return newList;
        }
        public CollectionPerEmpresa<T> Find(string empresa)
        {
            return this.List.Where(e => e.EmpresaFacturacion == empresa).FirstOrDefault();
        }
    }
}
