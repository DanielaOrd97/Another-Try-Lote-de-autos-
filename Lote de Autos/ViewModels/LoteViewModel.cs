using CommunityToolkit.Mvvm.Input;
using LoteAutos.Data;
using LoteAutos.Models;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;

namespace LoteAutos.ViewModels
{
    public class LoteViewModel:INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        AutoDBContext context;
        public List<Auto> ListaAutos { get; set; } = null!;
        public Auto AutoData { get; set; } = new();
        public static Auto AutoEdit { get; set; }
        public Auto Copy { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand VerAutoCommand {  get; set; }
        public LoteViewModel()
        {
            context = new AutoDBContext();
            VerAutoCommand = new AsyncCommand<int>(VerAuto);
            AgregarCommand = new AsyncCommand(AgregarAuto);
            EditarCommand = new AsyncCommand(EditarAuto);
            EliminarCommand = new AsyncCommand<int>(EliminarAuto);
            VerTodos();
            Copy = AutoEdit;
        }

        public async Task VerAuto(int id)
        {
            if(id >= 0)
            {
               AutoData = await context.GetById(id);
               AutoEdit = AutoData;
            }
        }

        public async Task EliminarAuto(int id)
        {
           if(id >= 0)
           {
                await context.Eliminar(id);
                VerTodos();
                OnPropertyChanged(nameof(ListaAutos));
            }
        }

        public async Task EditarAuto()
        {
            if(AutoEdit != null)
            {

                AutoData = new Auto
                {
                    Id = AutoEdit.Id,
                    Marca = AutoData.Marca ?? AutoEdit.Marca,
                    Modelo = AutoData.Modelo ?? AutoEdit.Modelo,
                    Version = AutoData.Version ?? AutoEdit.Version,
                    Año = AutoData.Año <= 0 ? AutoEdit.Año : AutoData.Año,
                    Precio = AutoData.Precio <= 0 ? AutoEdit.Precio : AutoData.Precio,
                    Kilometraje = AutoData.Kilometraje <= 0 ? AutoEdit.Kilometraje : AutoData.Kilometraje,
                    Motor = AutoData.Motor ?? AutoEdit.Motor,
                    Transmision = AutoData.Transmision ?? AutoEdit.Transmision,
                    Carroceria = AutoData.Carroceria ?? AutoEdit.Carroceria,
                    Descripcion = AutoData.Descripcion ?? AutoEdit.Descripcion
                };

                await context.Actualizar(AutoData);
                OnPropertyChanged(nameof(ListaAutos));
            }
        }

        public async Task VerTodos()
        {
            await context.GetAll();
            ListaAutos = context.ListaAutos;
            //foreach (var img in ListaAutos)
            //{
            //    //img.Imagen = HttpUtility.UrlEncode(img.Imagen);
            //    img.Imagen = HttpUtility.UrlDecode(img.Imagen);
            //}
            OnPropertyChanged(nameof(ListaAutos));
        }

        public async Task AgregarAuto()
        {
            if(AutoData != null)
            {
              await context.Agregar(AutoData);
              OnPropertyChanged(nameof(ListaAutos));
            }
        }

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
