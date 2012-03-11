function rocStruct = ROC_Curve(cc, tf, tc, div, tp)
%% cc = classifier
%% tf = feature vector
%% tc = classes
divisions=div;
testPulls=tp;
n_excer=size(tf,1);
default_tt=[10, 15, 20, 30, 40, 50];
testTimes=[roundn(n_excer/divisions,1):roundn(n_excer/divisions,1):n_excer,n_excer];
testTimes=unique([default_tt,testTimes]);

acc_ROC=zeros(length(testTimes),6);
for i =1:length(testTimes)-1
	acc_sub=zeros(testPulls,5);
	for j=1:testPulls
		rs=randperm(n_excer); rs=rs(1:testTimes(i));
		tf_sub=tf(rs,:);
		tc_sub=tc(rs,:);
		[wAcc, eAcc, oAcc, o2Acc, dAcc, ccAcc]=...
			LeaveOneOutValid(cc, tf_sub, tc_sub);
		acc_sub(j,:)=[wAcc, eAcc, oAcc, o2Acc, dAcc, ccAcc];
	end
	acc_ROC(i,:)=mean(acc_sub);
end
acc_ROC(length(testTimes),:)=LeaveOneOutValid(cc, tf_sub, tc_sub);

rocStruct=struct();
rocStruct.roc_levels=testTimes;
rocStruct.roc_acc=acc_ROC;
rocStruct.roc_accNames={'weighted', 'exact', 'offByOne', 'offByOneDim', 'distance', 'eachClass'};

end
