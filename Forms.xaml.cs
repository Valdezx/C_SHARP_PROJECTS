namespace taekwondo_app;

public partial class Forms : ContentPage
{
    // Початкові значення балів для Синього бійця
    private int _blueTech = 10;
    private int _blueRhythm = 6;
    private int _blueBalance = 6;
    private int _blueBreath = 6;
    private int _bluePower = 6;

    // Початкові значення балів для Червоного бійця
    private int _redTech = 10;
    private int _redRhythm = 6;
    private int _redBalance = 6;
    private int _redBreath = 6;
    private int _redPower = 6;

    public Forms()
    {
        InitializeComponent();
        UpdateUI();
    }

    #region Кліки Синього бійця (-1 бал)

    private void BlueTech_Clicked(object sender, EventArgs e) { if (_blueTech > 0) _blueTech--; UpdateUI(); }
    private void BlueRhythm_Clicked(object sender, EventArgs e) { if (_blueRhythm > 0) _blueRhythm--; UpdateUI(); }
    private void BlueBalance_Clicked(object sender, EventArgs e) { if (_blueBalance > 0) _blueBalance--; UpdateUI(); }
    private void BlueBreath_Clicked(object sender, EventArgs e) { if (_blueBreath > 0) _blueBreath--; UpdateUI(); }
    private void BluePower_Clicked(object sender, EventArgs e) { if (_bluePower > 0) _bluePower--; UpdateUI(); }

    #endregion

    #region Кліки Червоного бійця (-1 бал)

    private void RedTech_Clicked(object sender, EventArgs e) { if (_redTech > 0) _redTech--; UpdateUI(); }
    private void RedRhythm_Clicked(object sender, EventArgs e) { if (_redRhythm > 0) _redRhythm--; UpdateUI(); }
    private void RedBalance_Clicked(object sender, EventArgs e) { if (_redBalance > 0) _redBalance--; UpdateUI(); }
    private void RedBreath_Clicked(object sender, EventArgs e) { if (_redBreath > 0) _redBreath--; UpdateUI(); }
    private void RedPower_Clicked(object sender, EventArgs e) { if (_redPower > 0) _redPower--; UpdateUI(); }

    #endregion

    #region Автоматична калькуляція та оновлення UI

    private void UpdateUI()
    {
        // Автоматичний розрахунок загальної суми
        int blueTotal = _blueTech + _blueRhythm + _blueBalance + _blueBreath + _bluePower;
        int redTotal = _redTech + _redRhythm + _redBalance + _redBreath + _redPower;

        BlueTotalLabel.Text = blueTotal.ToString();
        RedTotalLabel.Text = redTotal.ToString();

        // Оновлення тексту на кнопках із по поточними балами
        BlueTechBtn.Text = $"Техн. зміст: {_blueTech} / 10 (-1)";
        BlueRhythmBtn.Text = $"Ритм: {_blueRhythm} / 6 (-1)";
        BlueBalanceBtn.Text = $"Баланс: {_blueBalance} / 6 (-1)";
        BlueBreathBtn.Text = $"Дихання: {_blueBreath} / 6 (-1)";
        BluePowerBtn.Text = $"Потужність: {_bluePower} / 6 (-1)";

        RedTechBtn.Text = $"Техн. зміст: {_redTech} / 10 (-1)";
        RedRhythmBtn.Text = $"Ритм: {_redRhythm} / 6 (-1)";
        RedBalanceBtn.Text = $"Баланс: {_redBalance} / 6 (-1)";
        RedBreathBtn.Text = $"Дихання: {_redBreath} / 6 (-1)";
        RedPowerBtn.Text = $"Потужність: {_redPower} / 6 (-1)";
    }

    // Визначення переможця
    private async void OnDetermineWinnerClicked(object sender, EventArgs e)
    {
        int blueTotal = _blueTech + _blueRhythm + _blueBalance + _blueBreath + _bluePower;
        int redTotal = _redTech + _redRhythm + _redBalance + _redBreath + _redPower;

        string message;
        if (blueTotal > redTotal)
            message = $"🟦 Переміг СИНІЙ кут!\n\nСиній: {blueTotal} балів\nЧервоний: {redTotal} балів";
        else if (redTotal > blueTotal)
            message = $"🟥 Переміг ЧЕРВОНИЙ кут!\n\nЧервоний: {redTotal} балів\nСиній: {blueTotal} балів";
        else
            message = $"🤝 НІЧИЯ!\nОбидва спортсмени набрали по {blueTotal} балів";

        await DisplayAlertAsync("Результат тулів", message, "ОК");
    }

    // Скидання балів до початкових 34
    private async void OnResetClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync("Скидання", "Відновити всі бали до стандартних (34)?", "Так", "Ні");
        if (confirm)
        {
            _blueTech = 10; _blueRhythm = 6; _blueBalance = 6; _blueBreath = 6; _bluePower = 6;
            _redTech = 10; _redRhythm = 6; _redBalance = 6; _redBreath = 6; _redPower = 6;

            UpdateUI();
        }
    }

    // Повернення на головну сторінку
    private async void OnBackToMainClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    #endregion
}
