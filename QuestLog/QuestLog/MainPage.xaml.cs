using System.Text.Json;
using System.Collections.ObjectModel;

namespace QuestLog;

public partial class MainPage : ContentPage
{
    public ObservableCollection<Quest> Quests { get; } = new ObservableCollection<Quest>();

    int next = 0;
    string path = Path.Combine(FileSystem.AppDataDirectory, "quests.json");

    Quest selectedQuest;

    public MainPage()
    {
        InitializeComponent();
        Load();

        BindingContext = this;

        CountLabel.Text = $"{Quests.Count} quests";
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        if (int.TryParse(RewardEntry.Text, out int reward))
        {
            Quests.Add(new Quest { Title = TitleEntry.Text, Giver = GiverEntry.Text, Reward = int.Parse(RewardEntry.Text) });
        }
        else
        {
            RewardEntry.Text = "Please Enter a Number!";
        }
        
        CountLabel.Text = $"{Quests.Count} quests";
        Save();
    }

    

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        Quests.Remove(selectedQuest);
        Save();
    }

    private void OnQuestSelect(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0) return;

        selectedQuest = (Quest)e.CurrentSelection[0];
        QuestView.SelectedItem = null;
    }

    void Save()
    {
        string json = JsonSerializer.Serialize(Quests);
        File.WriteAllText(path, json);
    }

    void Load()
    {
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        List<Quest> saved = JsonSerializer.Deserialize<List<Quest>>(json);

        foreach (Quest q in saved) Quests.Add(q);
    }
}
