namespace taekwondo_app;

public partial class Sparring : ContentPage
{
    // Клас для збереження історії дій (для можливості Undo)
    private class ActionRecord
    {
        public int PointsChange { get; set; }
        public int WarningsChange { get; set; }
        public int PenaltyPointsChange { get; set; }
        public string Description { get; set; } = string.Empty;
        public View? HistoryLabel { get; set; }
    }

    private int _blueTotal = 0;
    private int _blueWarnings = 0;
    private int _bluePenalties = 0;
    private readonly Stack<ActionRecord> _blueHistory = new();

    private int _redTotal = 0;
    private int _redWarnings = 0;
    private int _redPenalties = 0;
    private readonly Stack<ActionRecord> _redHistory = new();

    public Sparring()
    {
        InitializeComponent();
    }

    #region Логіка синього бійця (Blue)

    private void OneBluePoint_Clicked(object sender, EventArgs e) => AddBlueAction(1, 0, 0, "+1 бал");
    private void TwoBluePoint_Clicked(object sender, EventArgs e) => AddBlueAction(2, 0, 0, "+2 бали");
    private void ThreeBluePoint_Clicked(object sender, EventArgs e) => AddBlueAction(3, 0, 0, "+3 бали");
    private void FourBluePoint_Clicked(object sender, EventArgs e) => AddBlueAction(4, 0, 0, "+4 бали");
    private void FiveBluePoint_Clicked(object sender, EventArgs e) => AddBlueAction(5, 0, 0, "+5 балів");

    private void BlueMinusOne_Clicked(object sender, EventArgs e) => AddBlueAction(-1, 0, 1, "-1 бал (штраф)");

    private void BlueWarning_Clicked(object sender, EventArgs e)
    {
        int newWarnings = 1;
        int penaltyToAdd = 0;
        int pointsDeduction = 0;

        // Перевіряємо, чи це третє зауваження
        if ((_blueWarnings + 1) % 3 == 0)
        {
            penaltyToAdd = 1;
            pointsDeduction = -1; // 3 зауваження = -1 бал
        }

        string desc = penaltyToAdd > 0
            ? "Зауваження (3-тє -> -1 бал)"
            : "Зауваження";

        AddBlueAction(pointsDeduction, newWarnings, penaltyToAdd, desc);
    }

    private void AddBlueAction(int pts, int warnings, int penalties, string desc)
    {
        _blueTotal += pts;
        _blueWarnings += warnings;
        _bluePenalties += penalties;

        UpdateBlueUI();

        var label = CreateHistoryLabel($"{desc} (всього: {_blueTotal})", Color.FromArgb("#1E88E5"));
        BlueHistoryLayout.Children.Add(label);

        _blueHistory.Push(new ActionRecord
        {
            PointsChange = pts,
            WarningsChange = warnings,
            PenaltyPointsChange = penalties,
            Description = desc,
            HistoryLabel = label
        });
    }

    private void BlueUndo_Clicked(object sender, EventArgs e)
    {
        if (_blueHistory.Count == 0) return;

        var lastAction = _blueHistory.Pop();
        _blueTotal -= lastAction.PointsChange;
        _blueWarnings -= lastAction.WarningsChange;
        _bluePenalties -= lastAction.PenaltyPointsChange;

        if (lastAction.HistoryLabel != null)
            BlueHistoryLayout.Children.Remove(lastAction.HistoryLabel);

        UpdateBlueUI();
    }

    private void UpdateBlueUI()
    {
        BlueTotalLabel.Text = _blueTotal.ToString();
        BluePenaltiesLabel.Text = $"Штрафи: -{_bluePenalties} | Зауваження: {_blueWarnings % 3}/3 (всього: {_blueWarnings})";
    }

    #endregion

    #region Логіка червоного бійця (Red)

    private void OneRedPoint_Clicked(object sender, EventArgs e) => AddRedAction(1, 0, 0, "+1 бал");
    private void TwoRedPoint_Clicked(object sender, EventArgs e) => AddRedAction(2, 0, 0, "+2 бали");
    private void ThreeRedPoint_Clicked(object sender, EventArgs e) => AddRedAction(3, 0, 0, "+3 бали");
    private void FourRedPoint_Clicked(object sender, EventArgs e) => AddRedAction(4, 0, 0, "+4 бали");
    private void FiveRedPoint_Clicked(object sender, EventArgs e) => AddRedAction(5, 0, 0, "+5 балів");

    private void RedMinusOne_Clicked(object sender, EventArgs e) => AddRedAction(-1, 0, 1, "-1 бал (штраф)");

    private void RedWarning_Clicked(object sender, EventArgs e)
    {
        int newWarnings = 1;
        int penaltyToAdd = 0;
        int pointsDeduction = 0;

        if ((_redWarnings + 1) % 3 == 0)
        {
            penaltyToAdd = 1;
            pointsDeduction = -1; // 3 зауваження = -1 бал
        }

        string desc = penaltyToAdd > 0
            ? "Зауваження (3-тє -> -1 бал)"
            : "Зауваження";

        AddRedAction(pointsDeduction, newWarnings, penaltyToAdd, desc);
    }

    private void AddRedAction(int pts, int warnings, int penalties, string desc)
    {
        _redTotal += pts;
        _redWarnings += warnings;
        _redPenalties += penalties;

        UpdateRedUI();

        var label = CreateHistoryLabel($"{desc} (всього: {_redTotal})", Color.FromArgb("#E53935"));
        RedHistoryLayout.Children.Add(label);

        _redHistory.Push(new ActionRecord
        {
            PointsChange = pts,
            WarningsChange = warnings,
            PenaltyPointsChange = penalties,
            Description = desc,
            HistoryLabel = label
        });
    }

    private void RedUndo_Clicked(object sender, EventArgs e)
    {
        if (_redHistory.Count == 0) return;

        var lastAction = _redHistory.Pop();
        _redTotal -= lastAction.PointsChange;
        _redWarnings -= lastAction.WarningsChange;
        _redPenalties -= lastAction.PenaltyPointsChange;

        if (lastAction.HistoryLabel != null)
            RedHistoryLayout.Children.Remove(lastAction.HistoryLabel);

        UpdateRedUI();
    }

    private void UpdateRedUI()
    {
        RedTotalLabel.Text = _redTotal.ToString();
        RedPenaltiesLabel.Text = $"Штрафи: -{_redPenalties} | Зауваження: {_redWarnings % 3}/3 (всього: {_redWarnings})";
    }

    #endregion

    #region Допоміжні методи

    private Label CreateHistoryLabel(string text, Color color)
    {
        return new Label
        {
            Text = text,
            TextColor = color,
            FontSize = 14,
            FontAttributes = FontAttributes.Bold
        };
    }

    private async void OnResetClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync("Скидання", "Ви дійсно бажаєте скинути всі бали та штрафи?", "Так", "Ні");
        if (confirm)
        {
            _blueTotal = 0; _blueWarnings = 0; _bluePenalties = 0; _blueHistory.Clear();
            _redTotal = 0; _redWarnings = 0; _redPenalties = 0; _redHistory.Clear();

            UpdateBlueUI();
            UpdateRedUI();

            BlueHistoryLayout.Children.Clear();
            RedHistoryLayout.Children.Clear();
        }
    }
    private async void OnBackToMainClicked(object sender, EventArgs e)
    {
        // Повернення на MainPage у стеку навігації
        await Navigation.PopAsync();
    }
    #endregion
}