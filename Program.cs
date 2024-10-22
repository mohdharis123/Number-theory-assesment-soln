import json
from collections import defaultdict

def maximize_holidays(offices_within_100km, holidays_per_office):
    n_months = 12
    offices = list(holidays_per_office.keys())
    n_offices = len(offices)

    # Initialize dp and tracking table
    dp = [[0] * n_offices for _ in range(n_months)]
    track_moves = [[None] * n_offices for _ in range(n_months)]

    # Fill the first month's holidays
    for i in range(n_offices):
        dp[0][i] = holidays_per_office[offices[i]][0]

    # Function to get quarter number
    def get_quarter(month):
        return (month // 3)

    # Maximize holidays using dp
    for month in range(1, n_months):
        for i in range(n_offices):
            # Stay in the same office
            max_holidays = dp[month - 1][i] + holidays_per_office[offices[i]][month]
            track_moves[month][i] = i  # Same office
            
            # Check moves to other offices
            for j in range(n_offices):
                if offices[j] in offices_within_100km[offices[i]]:
                    # Check move constraints
                    if track_moves[month - 1][j] != i or(get_quarter(month) != get_quarter(month - 1)):
                        holidays_with_move = dp[month - 1][j] + holidays_per_office[offices[i]][month]
                        if holidays_with_move > max_holidays:
                            max_holidays = holidays_with_move
                            track_moves[month][i] = j


            dp[month][i] = max_holidays

    # Find the maximum holidays and the corresponding office sequence
    max_holidays = max(dp[-1])
    end_office = dp[-1].index(max_holidays)

    # Traceback to find the sequence of offices
    sequence_of_offices = [end_office]
    for month in range(n_months - 1, 0, -1):
        end_office = track_moves[month][end_office]
        sequence_of_offices.append(end_office)


    sequence_of_offices = sequence_of_offices[::- 1]
    sequence_of_offices = [offices[i] for i in sequence_of_offices]


    return sequence_of_offices, max_holidays

# Example input
offices_within_100km_json = '''
{
    "Noida": ["Delhi", "Gurugram","Faridabad"],
    "Delhi": ["Noida", "Gurugram", "Sonipat","Faridabad"],
    "Sonipat": ["Delhi", "Panipat", "Gurugram"],
    "Gurugram": ["Noida", "Delhi", "Sonipat","Panipat","Faridabad"],
    "Panipat": ["Sonipat", "Gurugram"],
    "Faridabad": ["Delhi","Noida","Gurugram"]
}
'''

holidays_per_office_json = '''
{
    "Noida": [1, 3, 4, 2, 1, 5, 6, 5, 1, 7, 2, 1],
    "Delhi": [5, 1, 8, 2, 1, 7, 2, 6, 2, 8, 2, 6],
    "Sonipat": [2, 5, 8, 2, 1, 6, 9, 3, 2, 1, 5, 7],
    "Gurugram": [6, 4, 1, 6, 3, 4, 7, 3, 2, 5, 7, 8],
    "Panipat": [2, 4, 3, 1, 7, 2, 6, 8, 2, 1, 4, 6],
    "Faridabad": [2, 4, 6, 7, 2, 1, 3, 6, 3, 1, 6, 8]
}
'''

offices_within_100km = json.loads(offices_within_100km_json)
holidays_per_office = json.loads(holidays_per_office_json)

sequence_of_offices, max_holidays = maximize_holidays(offices_within_100km, holidays_per_office)

print("Sequence of Offices:", sequence_of_offices)
print("Maximum Holidays:", max_holidays)

