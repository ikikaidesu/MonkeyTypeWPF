   M S B A M L   `   `   `  ����H  DMonkeyTypeWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:MonkeyTypeWPF.usercontrolsMonkeyTypeWPF.usercontrols  5      C&clr-namespace:MonkeyTypeWPF.modelviewsMonkeyTypeWPF.modelviews  5      *    $MonkeyTypeWPF.usercontrols.inputname   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:MonkeyTypeWPF.usercontrols     5      5vm&clr-namespace:MonkeyTypeWPF.modelviews     5      -   5          >�Name$  inputnick_uc��5	      .��5      � .+�5   
   �� ���#323437 �� CornerRadius$ 10��5      $	��190��5      $��40��5       �� Padding$ 5q�5      $
��Left=�5      ._�5      � .+�5      �� -   5         ��Name$ NameBox��5      
������ $��0q�5      ���#deddd0$��16(�5      $��Center=�5      $G��Apack://application:,,,/MonkeyTypeWPF;component/Fonts/#Roboto Mono)�5      $��1,0,0,0q�5       �� 	MaxLength$ 15��5      $
��auto��5       ��5      �� *
NickName+ �� ElementName$ inputnick_uc��5       �� 5"       J� IsHitTestVisible .  
�������$��20(�5&      $G��Apack://application:,,,/MonkeyTypeWPF;component/Fonts/#Roboto Mono)�5'      $��Center=�5(      ��5$      �� *Title+$ inputnick_uc����5)      �� 5*       �� 
TargetType" ��6     �� Triggers 5+      �� 5,   "   	 �� Binding	 6.   �� *Text+$ NameBox��
 �� Value
  6\   .a�5-   &   ��  �� Property � y� �� Value$ Visible=�6C   5.   #   .�50      ��  � y�$ Hidden=�6;   51      53      �� 55      $��Center=�6   $��Right=�56      
������ $��0q�58       � Cursor$
 Hand��59      ��5:      �� *	Command+$ inputnick_uc����5;      �� *Text+$ NameBox��.��5<      �� $��/images/check_mark.png��6   $��25��6<   $��25��6G   ��5>      �� 5?      " ��6    .�5@      ��  � ��
 ���� 6;   �� 5A       �@J�Opacity$ 0Q�68   �� 5B       �@J�IsHitTestVisible .  6A   �� 5C       � �� 5D   "   �� 5E   &    �� 
TargetType" ��66   .R�5F   *   ��  ��Name$ Border��61   #��z��6A   ._�5G   .   �� $��Center=�6?   $��Center=�6\   5H   +    �� Triggers 5I   *   �� 5J   .   $

 True��6u   	 6:   �� *IsKeyboardFocused+$ NameBox��.a�5K   2   ��  �@J�Opacity$ 1Q�6L   �� 5L   2    �@J�IsHitTestVisible . 6U   5M   /   P� 5N   .    P� Property �@J�IsMouseOver P� Value . . �5O   2   ��  �� 
TargetName$ Border��6c    � ��
 ���� 5P   /   5R   '   5T      5U      5W      5X      5Y      5Z      5[      