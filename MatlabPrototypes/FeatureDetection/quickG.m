function [out]=quickG(bayes, map, svm, lims)
	accN=1;
	bx=bayes.roc_levels;
	mx=map.roc_levels;
	sx=svm.roc_levels;
	by=bayes.roc_acc(:,accN)*100;
	my=map.roc_acc(:,accN)*100;
	sy=svm.roc_acc(:,accN)*100;
	
	l_lim=lims(1);
	u_lim=lims(2);
	fc=0.7;
	
	function [oroc_x, oroc_y] = prepareROC(roc_x, roc_y, lims, fc)
		interpSize=1;
		oroc_x=lims(1):interpSize:lims(2);
		oroc_y=interp1(roc_x,roc_y, lims(1):interpSize:lims(2), 'cubic');

		% Cut-off frequency (Hz) %% fc = 0.5; 
		fs = 20; % Sampling rate (Hz)
		order = 3; % Filter order
		[B,A] = butter(order,2*fc/fs); % [0:pi] maps to [0:1] here
		oroc_y=filtfilt(B,A,oroc_y); % LPF
	end

	[bx2,by2]=prepareROC(bx,by, [l_lim, u_lim], fc);
	[mx2,my2]=prepareROC(mx,my, [l_lim, u_lim], fc);
	[sx2,sy2]=prepareROC(sx,sy, [l_lim, u_lim], fc);
	
	out=[bx2',by2',my2',sy2'];
	

end
