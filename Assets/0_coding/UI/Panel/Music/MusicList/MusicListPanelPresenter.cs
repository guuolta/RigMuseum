public class MusicListPanelPresenter : PanelPresenterBase<MusicListPanelView>
{
    public void SetText(int id, string title)
    {
        View.IDText.text = id.ToString();
        View.TitleText.text = title;
    }
}