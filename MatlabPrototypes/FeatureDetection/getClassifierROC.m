%% Get ROCs for different classifiers

cc=MakeClassifier('GED', false);


divs=20;
pulls=25;
[tf,tc]=getTrainingData('squats', 1);
gedROC_squats=ROC_Curve(cc, tf, tc, divs, pulls);

[tf,tc]=getTrainingData('armRaise', 1);
gedROC_armRaise=ROC_Curve(cc, tf, tc, divs, pulls);

[tf,tc]=getTrainingData('legRaise', 1);
gedROC_legRaise=ROC_Curve(cc, tf, tc, divs, pulls);

[tf,tc]=getTrainingData('legExt', 1);
gedROC_legExt=ROC_Curve(cc, tf, tc, divs, pulls);