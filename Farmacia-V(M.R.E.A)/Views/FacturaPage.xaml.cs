namespace Farmacia_V_M.R.E.A_.Views;
using Farmacia_V_M.R.E.A_.Services; // O el espacio de nombres donde esté la clase DatabaseService


public partial class FacturaPage : ContentPage
{
    private readonly Factura _factura;
    private readonly DatabaseService _databaseService;

    public FacturaPage(Factura factura)
    {
        InitializeComponent();
        _factura = factura;
        _databaseService = new DatabaseService();
        LoadFactura();
    }

    private async void LoadFactura()
    {
        lblNumero.Text = $"Número: {_factura.NumeroFactura}";
        lblFecha.Text = $"Fecha: {_factura.Fecha:dd/MM/yyyy HH:mm}";

        var detalles = await _databaseService.GetDetalleFacturaAsync(_factura.Id);
        DetalleCollectionView.ItemsSource = detalles;

        lblSubtotal.Text = $"${_factura.Subtotal:F2}";
        lblDescuento.Text = $"${_factura.Descuento:F2}";
        lblImpuesto.Text = $"${_factura.Impuesto:F2}";
        lblTotal.Text = $"${_factura.Total:F2}";
    }

    private async void OnNuevaCompraClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnSalirClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}