%% Get ROCs for different classifiers

cc=MakeClassifier('GED', false);

[tf,tc]=GetTrainingData('squats', 1);
gedROC_squats=ROC_Curve(cc, tf, tc, 20, 25);

[tf,tc]=GetTrainingData('armRaise', 1);
gedROC_armRaise=ROC_Curve(cc, tf, tc, 20, 25);

[tf,tc]=GetTrainingData('legRaise', 1);
gedROC_legRaise=ROC_Curve(cc, tf, tc, 20, 25);

[tf,tc]=GetTrainingData('legExt', 1);
gedROC_legExt=ROC_Curve(cc, tf, tc, 20, 25);