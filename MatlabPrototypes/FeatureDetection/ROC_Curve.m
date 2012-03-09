function rocStruct = ROC_Curve(cc, tf, tc)
%% cc = classifier
%% tf = feature vector
%% tc = classes
divisions=12;
testPulls=12;
n_excer=size(tf,1);
testTimes=[roundn(n_excer/divisions,1):roundn(n_excer/divisions,1):n_excer,n_excer];

acc_ROC=zeros(length(testTimes),5);
for i =1:length(testTimes)-1
	acc_sub=zeros(testPulls,5);
	for j=1:testPulls
		rs=randperm(n_excer); rs=rs(1:testTimes(i));
		tf_sub=tf(rs,:);
		tc_sub=tc(rs,:);
		[wAcc, eAcc, oAcc, o2Acc, dAcc]=...
			LeaveOneOutValid(cc, tf_sub, tc_sub);
		acc_sub(j,:)=[wAcc, eAcc, oAcc, o2Acc, dAcc];
	end
	acc_ROC(i,:)=mean(acc_sub);
end
acc_ROC(length(testTimes),:)=LeaveOneOutValid(cc, tf_sub, tc_sub);

rocStruct=struct();
rocStruct.roc_levels=testTimes;
rocStruct.roc_acc=acc_ROC;
rocStruct.roc_accNames={'weighted', 'exact', 'offByOne', 'offByOneDim', 'distance'};

end
