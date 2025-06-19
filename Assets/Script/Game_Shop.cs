using Carrot;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Shop : MonoBehaviour
{
    [Header("obj Main")]
    public Game game;

    private IList<Sprite> list_icon_temp;
    public Item_Obj[] list_Item_Obj;
    private Item_Obj[] Items=null;
    private Carrot_Box box_shop = null;
    public void On_load()
    {
        if (Items == null)
        {
            Items = new Item_Obj[list_Item_Obj.Length];
            for (int i = 0; i < list_Item_Obj.Length; i++)
            {
                Items[i] = Instantiate(this.list_Item_Obj[i]);
            }
        }
    }

    public void Show()
    {
        this.game.carrot.play_sound_click();
        box_shop = this.game.carrot.Create_Box();
        box_shop.set_title("Shop");
        box_shop.set_icon(this.game.carrot.icon_carrot_all_category);
        
        foreach (Item_Obj item in this.Items)
        {
            Carrot_Box_Item item_obj = box_shop.create_item("in_app_item");
            item_obj.set_title(item.s_name);
            item_obj.set_icon_white(item.sp_icon);
            item_obj.set_tip(item.s_desc);
            if (item.index_buy == 0)
            {
                item_obj.set_act(() =>
                {
                    this.game.change_skin_player(item.sp_items[0],item.sp_items[1]);
                    this.game.carrot.play_sound_click();
                    box_shop.close();
                });
            }
            else
            {
                item_obj.set_act(() =>
                {
                    this.game.carrot.play_sound_click();
                    this.game.carrot.shop.buy_product(item.index_buy);
                });

                Carrot_Box_Btn_Item btn_buy = item_obj.create_item();
                btn_buy.set_icon(this.game.carrot.icon_carrot_buy);
                btn_buy.set_color(this.game.carrot.color_highlight);
                btn_buy.set_icon_color(UnityEngine.Color.white);
                Destroy(btn_buy.GetComponent<Button>());
            }
            
        }
    }
    
    public void OnPaySuccess(string id_p)
    {
        if (id_p ==this.game.carrot.shop.get_id_by_index(0))
        {
            this.game.carrot.Show_msg("Thank you for your support!");
        }else{
            for(int i=0;i<this.Items.Length;i++)
            {
                if (this.game.carrot.shop.get_id_by_index(this.Items[i].index_buy)== id_p)
                {
                    this.Items[i].index_buy= 0;
                    this.game.change_skin_player(this.Items[i].sp_items[0],this.Items[i].sp_items[1]);
                    this.game.carrot.Show_msg("Thank you buying " + this.Items[i].s_name + "!");
                    PlayerPrefs.SetInt("sel_skin", i);
                    if (box_shop != null) box_shop.close();
                    break;
                }
            }
        }
    }

}
