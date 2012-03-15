function bestFeatures(tf, tc, varargin)
	% optionaly find best features on specifc critComp
	indFeatEval=zeros(size(tf,2),3);
	icdFeatEval=zeros(size(tf,2),3);
	
	
	function [cc_n,features]=bestFeaturesFinder(fea_eval_in, varargin)
		fea_sort=sort(fea_eval_in,'descend');
		howMany=6;
		[~, cc_n]=max(sum(fea_sort(1:howMany, :)));
		if numel(varargin) > 0
			cc_n = varargin{1};
		end
		fprintf('Using criticalComp: %i\n', cc_n);
		find_fn=fea_sort(1:howMany, cc_n);
		features=zeros(howMany,2);
		for i=1:howMany
			f_ind=find(fea_eval_in(:,cc_n) == find_fn(i));
			features(i,:)=[f_ind,find_fn(i)];
		end
		features=flipud(sortrows(features,2));
	end


	for i=1:3
		indFeatEval(:,i)=sum(IndFeat3(tf, tc(:,i)).^2,1).^0.5;
		icdFeatEval(:,i)=sum(GetICD(tf, tc(:,i)).^2,1).^0.5;
	end
	if numel(varargin) > 0
		force_critComp = varargin{1};
		[~,ff]=bestFeaturesFinder(indFeatEval,force_critComp)
		[~,ff]=bestFeaturesFinder(icdFeatEval,force_critComp)
	else
		[~,ff]=bestFeaturesFinder(indFeatEval)
		[~,ff]=bestFeaturesFinder(icdFeatEval)
	end
end
	