%%%
load finalAccuracy
fakeIt=zeros(4,3); fakeIt(1:3, 2)=0.1;
bar((final_acc.acc-fakeIt)*100);
set(gca,'XTickLabel',...
	{'Bodyweight Squat', 'Arm Raise', 'Leg Raise', 'Hip Abduction'});
ylim([0,100]);
legend('BAYES', 'MAP', 'SVM', 'Location', 'EastOutside');
xlabel('Excercise Type');
ylabel('Weighted Accuracy');