function classifications = classifyLots(obj, testPoints)
	classifications=zeros(size(testPoints,1),3)-1; % -1 for debug
	for i = 1:size(testPoints,1)
		classifications(i,:)=obj.classify(testPoints(i,:));
	end		
end