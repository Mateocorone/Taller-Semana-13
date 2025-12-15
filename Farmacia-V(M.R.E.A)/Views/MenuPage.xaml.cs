using System.Collections.ObjectModel;
namespace Farmacia_V_M.R.E.A_.Views;
using Farmacia_V_M.R.E.A_.Services;




public partial class MenuPage : ContentPage
{
    private ObservableCollection<Medicina> _medicinas;
    private readonly DatabaseService _databaseService;

    public MenuPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        _medicinas = new ObservableCollection<Medicina>();
        MedicinasCollectionView.ItemsSource = _medicinas;
        LoadMedicinas();
    }

    private async void LoadMedicinas()
    {
        var medicinas = await _databaseService.GetMedicinasAsync();
        _medicinas.Clear();
        foreach (var medicina in medicinas)
        {
            medicina.PropertyChanged += OnMedicinaPropertyChanged;
            _medicinas.Add(medicina);
        }
    }

    private void OnMedicinaPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Medicina.Subtotal))
        {
            UpdateTotal();
        }
    }

    private void OnIncrementClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Medicina medicina)
        {
            medicina.Cantidad++;
        }
    }

    private void OnDecrementClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Medicina medicina)
        {
            if (medicina.Cantidad > 0)
            {
                medicina.Cantidad--;
            }
        }
    }

    private void UpdateTotal()
    {
        var total = _medicinas.Sum(m => m.Subtotal);
        lblTotal.Text = $"Total: ${total:F2}";
    }

    private async void OnGenerarFacturaClicked(object sender, EventArgs e)
    {
        var medicinasSeleccionadas = _medicinas.Where(m => m.Cantidad > 0).ToList();

        if (!medicinasSeleccionadas.Any())
        {
            await DisplayAlert("Error", "Seleccione al menos una medicina", "OK");
            return;
        }

        var factura = new Factura
        {
            NumeroFactura = $"FAC-{DateTime.Now:yyyyMMddHHmmss}",
            Fecha = DateTime.Now,
            Subtotal = medicinasSeleccionadas.Sum(m => m.Subtotal),
        };

        decimal descuento = 0;

        if (factura.Subtotal > 50)
        {
            descuento = factura.Subtotal * 0.05m;
        }

        factura.Descuento = descuento;
        factura.Impuesto = (factura.Subtotal - descuento) * 0.12m;
        factura.Total = factura.Subtotal - descuento + factura.Impuesto;


        var facturaId = await _databaseService.SaveFacturaAsync(factura, medicinasSeleccionadas);
        factura.Id = facturaId;

        await Navigation.PushAsync(new FacturaPage(factura));
    }
}