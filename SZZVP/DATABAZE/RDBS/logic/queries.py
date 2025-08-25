from config import get_connection

def get_summary_by_country():
    conn = get_connection()
    cur = conn.cursor()
    cur.execute("""
        SELECT c.country, COUNT(*) AS count
        FROM foreigners c
        GROUP BY c.country
        ORDER BY count DESC
        LIMIT 10
    """)
    data = cur.fetchall()
    cur.close()
    conn.close()
    return data
