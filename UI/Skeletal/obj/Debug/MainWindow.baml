   M S B A M L   `   `   `  ����Z  VVisiblox.Charts, Version=2.2.0.38099, Culture=neutral, PublicKeyToken=1543c03f04c4461bJ6clr-namespace:Visiblox.Charts;assembly=Visiblox.ChartsVisiblox.Charts  5   	   | xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting 5   	    SkeletalViewer   SkeletalViewer.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Icharts6clr-namespace:Visiblox.Charts;assembly=Visiblox.Charts     5   	   �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5   	   -   5          � Title$"  Kinect Skeletal Viewer (WPF)��5   	    � Icon$ SkeletalViewer.ico��6.    � WindowStartupLocation$ CenterScreen=�5   	    � WindowState$ 	Maximized=�5   7    � 	Resources 5	         ScrollingItemsControl �� 
TargetType �� Padding J� SnapsToDevicePixels��%�   &	        �� 5   
   " ��68   .R�5      �� #��z��6   # z��6P   #��z��5      #��z��6H    . ._�5      �� $
��Auto=�6   .��5      �� # z 6%   5      5      5      5      .��5      � -   5        $��Stretch=�5      
������  � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   � $��0��6   	 � RowDefinitions	 5      �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5       $
��Auto �6    �� 5!      $
��Auto �6    �� 5"      $
��Auto �6    �� 5#      $
��Auto �6    �� 5$      $
��Auto �6    .+�5&      c� -   5        $��tb��5&   #   $��0��6   .@�5'      �� -   5        .��5(      �� $-��'/SkeletalViewer;component/Home-icon.png��6   6O   5)      5*      � 5+      
 � Orientation$
 
Horizontal=�6   $��1��61   .+�5,      ��  �� Orientation$ 
Horizontal=�6   .+�5-      �� $��Name:��6   $��32(�6,   6<   �� 5.      -   5        $
��user��5.      $��32(�6(   68   5/      50      � 51      $
 
Horizontal=�6   $��2��61   .+�52      �� $ 
Horizontal=�6   .+�53      �� $��	Exercise:��6   $��32(�60   6@   �� 54      -   5        $��exercisesCB��54      $	��230��62   $��32(�6n   $
��Left=�6|   6�   55      �� 56      $ 
Horizontal=�6   .+�57      �� $��Reps:��6   $��32(�6,   6<   �� 58      -   5        $��repsCB��58   1    J� 	AllowDrop .  $��60��6?   {�.  $��32(�6\   6l   59      �� 5:      $ 
Horizontal=�6   .+�5;      �� -   5        $��sample��5;      $��Sample��6+   $��Center=�6<   $	��150��6Y   
�����F�{�.  $��32(�6�   �� 5<      -	   5        $��button1��5<      $��Start��6,   $��Center=�6<   $	��150��6Z   ���	#FF009200{�.  $��32(�6�   5=      5>      � 5?      $
 
Horizontal=�6   $��3��61   .+�5@      �� $ 
Horizontal=�6   .+�5A      �� $��Last Performed:��6   $��32(�66   6F   �� 5B      -
   5        $
��date��5B      $	��N/A��6(   $��32(�66   6F   5C      5D         Visiblox.Charts.Chart  5E      -   5        $��	userchart��5E   (   $��4��6   $	��260��69      Visiblox.Charts.ChartBase   Title$ Performance��6F      XAxis 5F          Visiblox.Charts.LinearAxis  5G      �   ~Visiblox.Charts.AxisCommonBase`1[[System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Title$	 Rep��6(   ~   xVisiblox.Charts.AxisBase`1[[System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   ShowMajorGridlines .     ShowMajorTicks .    ShowMinorTicks .     MajorTickInterval$ 1Q�6|   5H         YAxis 5J        5K      $ Score��6(    .  $ 1Q�6M      Range 5L      !   Visiblox.Charts.DoubleRange  5M         yVisiblox.Charts.RangeBase`1[[System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Minimum$ 0Q�61      Maximum$ 2Q�6=   6K   5O         Series 5R      "   Visiblox.Charts.ColumnSeries    5S        5T      5V      �� 5X      -   5        $��feedback��5X   5   $��	Feedback:��6   $��5��6(   $��32(�6E   6U   � 5Z      $
 
Horizontal=�6   $��6��61   .+�5[      �� $ Vertical=�6   .+�5\      �� -   5        $��criticalComponent1��5\       $��32(�6:   $��WrapWithOverflow=�6H   6j   �� 5]      -   5        $��	buttoncc1��5]      $��Audio��6.   $��Center=�6>   $	��150��6[   
�����F�{�.  $��32(�6�   6�   5^      5_      � 5a      $
 
Horizontal=�6   $��7��61   .+�5b      �� $ Vertical=�6   .+�5c      �� -   5        $��criticalComponent2��5c       $��32(�6:   $��WrapWithOverflow=�6H   6j   �� 5d      -   5        $��	buttoncc2��5d      $��Audio��6.   $��Center=�6>   $	��150��6[   
�����F�{�.  $��32(�6�   6�   5e      5f      � 5h      $
 
Horizontal=�6   $��8��61   .+�5i      �� $ Vertical=�6   .+�5j      �� -   5        $��criticalComponent3��5j       $��32(�6:   $��WrapWithOverflow=�6H   5k      �� 5l      -   5        $��	buttoncc3��5l      $��Audio��6.   $��Center=�6>   $	��150��6[   
�����F�{�.  $��32(�6�   6�   5m      5n      5p      � 5q   
   $��1��6   .+�5s      �� -   5        $��viewerHolder��5s      #��[  6/   $��Center=�6a   5v      5x      5y      