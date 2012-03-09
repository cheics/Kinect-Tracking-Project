function [wAcc, eAcc, oAcc, o2Acc, dAcc] = LeaveOneOutValid(cl, tf, tc)
	% cl:	classifier obj
	% tf:	features
	% tf:	classes
	
	% wAcc: Weighted accuracy
	% eAcc: Exact classification 
	% oAcc: Off by one (222 -> 212)
	% o2Acc: Off by one dim (222 -> 111)
	
	function dd=distAcc(c1, c2, pow)
		dn2=sum([2,2,2].^pow).^(1/pow);
		dd=(sum(abs(c1-c2).^pow)^(1/pow))/dn2;
	end

	acc=zeros(size(tf,1),3);
	dAcc=zeros(size(tf,1),1);
	
	for i = 1:size(tf,1)
		trainData=[tf(1:i-1,:);tf(i+1:end,:)];
		trainClasses=[tc(1:i-1,:);tc(i+1:end,:)];
		cl.trainClassifiers(trainData,trainClasses);
		outClass=cl.classify(tf(i,:));
		dAcc(i)=1-distAcc(outClass, tc(i,:), 2);
		%% Check each crit-comp
		for j = 1:length(outClass)
			correctClass=tc(i,j);
			if correctClass==outClass(j)
				acc(i,j)=1;
			elseif abs(correctClass-outClass(j)) == 1
				acc(i,j)=0.5;
			end
		end
	end
	n=size(tf,1);
	wAcc=sum(sum(acc))/(n*3);
	eAcc=sum(ismember(acc, [1,1,1], 'rows'))/n;
	oAcc=length(find(sum(acc,2)==2.5))/n;
	
	[r,c]=find(acc>=0.5);
	[u_items,u_count]=count_unique(r);
	o2Acc=length(find(u_count==3))/n;
	
	dAcc=sum(dAcc)/n;
	
	

end