   M S B A M L   `   `   `  ����K  GCardReaderClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CardReaderClientCardReaderClient  5      )    #CardReaderClient.StudentsToSubjects   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:CardReaderClient     5        N� Title$  StudentsToSubjects��5      $	��426��6"   $	��392��6.   .��5
      � -   5        $��0,0,-163,-22q�5
       .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5         
IsReadOnly . $
��Left=�68   $	��144��6S   $��	10,26,0,0q�6`   $	��Top=�6s   $
��Auto��6�   ��5      �� 5       �� 
TargetType)  #System.Windows.Controls.DataGridRow"  6   .�5      -   5        5      5        5   
   -   5        $ dataGrid_Copy��5       . $
��Left=�6=   $	��144��6X   $��
10,206,0,0q�6e   $	��Top=�6y   $
��Auto��6�    J� RenderTransformOrigin$ 1.407,0.469&�6�   ��5      �� 5      "  6   .�5      -   5        5      5      �� 5   
   -   5         ��Name$ label��5      $��Studenci��6   $
��Left=�62   $��10,0,0,0q�6M   $	��Top=�6_   $ 0.439,0.487&�6w   �� 5   
   -   5        $ 
label_Copy��5      $��
Przedmioty��6$   $
��Left=�69   $��
10,175,0,0q�6T   $	��Top=�6h   $ 0.439,0.487&�6�   �� 5   
   -   5         ��Name$ button��5      $��Dodaj��6!   $
��Left=�61   $��46��6L   $��	0,368,0,0q�6X   $	��Top=�6k   $	��102��6�   6�   �� 5   
   -	   5        $ button_Copy��5      $��Usuń��6&   $
��Left=�65   $��46��6P   $��257,368,0,0q�6\   $	��Top=�6q   $	��102��6�   6�   �� 5   
   -
   5        $ button1��5      $��	Wyczyść��6"   $
��Left=�64   $��46��6O   $��128,368,0,0q�6[   $	��Top=�6p   $��96��6�   $ 0.375,-0.562&�6�   6�   5      5       